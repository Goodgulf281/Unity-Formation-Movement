using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Util;
using com.t7t.utilities;

namespace com.t7t.formation
{
    /*
     * This is a trigger placed on an empty object which influences nearby formations and (temporarily) changes their GridTypes. 
     * 
     * 
     * 
     * 
     * 
     * 
     */
    
    public class FormationAndState
    {
        public FormationGrid    formation;
        public GridTypes        oldGridType;
    }


    public class FormationGridTrigger : MonoBehaviour
    {

        [SerializeField] GridTypes  newGridType = GridTypes.None;
        [SerializeField] float      range = 5.0F;
        [SerializeField] float      waitForMove = 3.0F;
        [SerializeField] float      period = 0.8F;  // Checks every period seconds if a formation is in range
        [SerializeField] bool       revertOnExit = true;

        [SerializeField] protected  List<FormationAndState> formationAndStates; // Formations in range with their original states

        [SerializeField] bool       drawGizmos = true;

        private float time = 0.0f;
        private Toolbox toolbox;

        void Awake()
        {
            toolbox = Toolbox.Instance;

            formationAndStates = new List<FormationAndState>();
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

                        //Debug.Log("IN RANGE");

                        GridTypes gt = fg.GetGridType();
                        if (gt != newGridType)
                        {
                            FormationAndState fas = new FormationAndState();
                            fas.formation = fg;
                            fas.oldGridType = fg.GetGridType();

                            formationAndStates.Add(fas);

                            fg.ChangeGridTo(newGridType);
                            fg.ChangeState(FormationStates.Form);

                            // start coroutine to move in waitformove seconds
                            StartCoroutine("WaitAndStartMoving", fg);
                        }
                    }
                }

                // check if any of the in range formations is now out of range

                if (revertOnExit)
                {
                    //Debug.Log("FAS Count="+ formationAndStates.Count);

                    for (int i = formationAndStates.Count; i > 0; i--)
                    {
                        FormationAndState fas = formationAndStates[i-1];
                        FormationGrid fg = fas.formation;

                        //Debug.Log(Vector3.Distance(fg.transform.position, transform.position));

                        if (Vector3.Distance(fg.transform.position, transform.position) > range)
                        {
                            //Debug.Log("OUT OF RANGE");

                            fg.ChangeGridTo(fas.oldGridType);
                            fg.ChangeState(FormationStates.Form);

                            // start coroutine to move in waitformove seconds
                            StartCoroutine("WaitAndStartMoving", fg);
                            formationAndStates.RemoveAt(i - 1);
                        }
                    }
                }
            }
        }

        IEnumerator WaitAndStartMoving(FormationGrid fg)
        {
            yield return new WaitForSeconds(waitForMove);

            fg.ChangeState(FormationStates.Move);
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
