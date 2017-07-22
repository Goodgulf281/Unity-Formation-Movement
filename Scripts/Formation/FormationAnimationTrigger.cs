using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Util;
using com.t7t.utilities;

namespace com.t7t.formation
{

    public class FormationAnimationTrigger : MonoBehaviour
    {
        [SerializeField]    float period = 0.8F;  // Checks every period seconds if a formation is in range
        [SerializeField]    float range = 5.0F;
        [SerializeField]    bool drawGizmos = true;

        [Header("Animation")]

        [SerializeField]    Animator animator;
        [SerializeField]    string animationParameter;
        [SerializeField]    bool newState = true;


        private float time = 0.0f;
        private Toolbox toolbox;


        private void Awake()
        {
            toolbox = Toolbox.Instance;
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            time += Time.deltaTime;
            if (time >= period)
            {
                time = 0.0f;

                // check if a formation is now in range

                for (int i = 0; i < toolbox.allFormations.Count; i++)
                {
                    FormationGrid fg = toolbox.allFormations[i];

                    //Debug.Log(Vector3.Distance(fg.transform.position, transform.position));

                    if (Vector3.Distance(fg.transform.position, transform.position) < range)
                    {

                        animator.SetBool(animationParameter, newState);

                    }
                }


            }

        }


        void OnDrawGizmosSelected()
        {
            if (drawGizmos)
            {
                Draw.Gizmos.CircleXZ(transform.position, range, Color.yellow);

            }
        }

    }

}