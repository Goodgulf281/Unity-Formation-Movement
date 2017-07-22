using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
#if T7T_ASTAR
using Pathfinding;
#endif

namespace com.t7t.formation
{

    /*
     * This is the Anchor of the FormationGrid and is derived from AIPath since this will be following the path while the grid/gridpoints follow the anchor (with some "delay").
     * Add this script to an empty game object and link it to the FormationGrid. The FormationAnchor will move towards the FormationGrid in the FormationGrid.Awake() method.
     * 
     * Key properties: TargetReached.
     * 
     * Key public methods: OnTargetReached, GetVelocity
     * 
     */

#if T7T_ASTAR
    [RequireComponent(typeof(Seeker))]
    public class FormationAnchor : AIPath
#else
    [RequireComponent(typeof(NavMeshAgent))]
    public class FormationAnchor : MonoBehaviour
#endif
    {

        [SerializeField] protected UnityEvent TargetReached;

        protected FormationGrid myFormation;

#if T7T_ASTAR
        // Put A*Star specific properties here
#else
        private NavMeshAgent agent;

        public float endReachedDistance = 0.2F;

        public Transform target;


        private Vector3 previousPosition;
        private Vector3 curSpeed;

#endif

        // Use this for initialization
        void Start()
        {
#if T7T_ASTAR
            base.Start();
#else
            agent = GetComponent<NavMeshAgent>();
#endif
        }

        // Update is called once per frame
        void Update()
        {
#if T7T_ASTAR
            base.Update();

#else
            // Check if agent has reached destination. See answers.unity3d.com 324589

            if (myFormation)
            {
                if (myFormation.GetState() == FormationStates.Move)
                {
                    if (!agent.pathPending)
                    {
                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                            {

                                myFormation.ChangeState(FormationStates.Arrive);

                                TargetReached.Invoke(); // execute the Unity Event (attached through the inspector)

                            }
                        }
                    }
                }
            }
            // calculate real agent speed:
            Vector3 curMove = transform.position - previousPosition;
            curSpeed = curMove / Time.deltaTime;
            previousPosition = transform.position;
#endif
        }

        // This event is called by AIPath once the destination in the path is reached. Once this happens stop the searching for new paths and movement.
        // Lastly the TargetReached event is invoked. The FormationGrid may have hooked into this event to stop unit animations. You can also add your own
        // event hook to act on the anchor reachhig the end point.

#if T7T_ASTAR
        public override void OnTargetReached()
        {
            Debug.Log("FormationAnchor: Target Reached");

            canSearch = false;
            canMove = false;

            if(myFormation)
                myFormation.ChangeState(FormationStates.Arrive);

            TargetReached.Invoke(); // execute the Unity Event (attached through the inspector)
        }
#endif

        // Return the velocity of the anchor which is a property of the AIPath class.
        public Vector3 GetVelocity()
        {
#if T7T_ASTAR
            return velocity;
#else
            //return agent.velocity;
            return curSpeed;
#endif
        }

        public virtual void StartMove()
        {
#if T7T_ASTAR
            // TODO: Added equivalent A* Code here
#else
            agent.destination = target.position;
            agent.Resume();
#endif
        }

        public virtual void StartMove(GameObject _target)
        {
#if T7T_ASTAR
            // TODO: Added equivalent A* Code here
#else
            target = _target.transform;
            agent.destination = target.position;
#endif
        }

        public virtual void StopMove()
        {
#if T7T_ASTAR
            // TODO: Added equivalent A* Code here
#else
            agent.Stop();
#endif
        }

        public void SetFormation (FormationGrid formationgrid)
        {
            myFormation = formationgrid;
        }

        public FormationGrid GetFormation ()
        {
            return myFormation;
        }

    }
}
