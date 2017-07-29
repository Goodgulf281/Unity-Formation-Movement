using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.t7t.formation;
//using Pathfinding.Examples; // Code from A*Pathfinding

public class FormationSample : MonoBehaviour
{

    public FormationGrid formationGrid;
    //public AstarSmoothFollow2 smoothFollow;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log("Disband!");
            formationGrid.ChangeState(FormationStates.Disband);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Get moving!");
            // get the grid moving
            formationGrid.ChangeState(FormationStates.Move);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            // assign knights to the grid

            List<GameObject> list = new List<GameObject>();

            GameObject go;

            go = GameObject.Find("Knight (1)");
            list.Add(go);
            go = GameObject.Find("Knight (2)");
            list.Add(go);
            go = GameObject.Find("Knight (3)");
            list.Add(go);

            formationGrid.AssignObjectsToGrid(list);
            formationGrid.ChangeState(FormationStates.Form);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("Changing to Wedge!");

            formationGrid.ChangeGridTo(GridTypes.Wedge9);
            formationGrid.ChangeState(FormationStates.Form);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("Changing to Column!");

            formationGrid.ChangeGridTo(GridTypes.Column10);
            formationGrid.ChangeState(FormationStates.Form);
        }

/* This code uses the Smooth follow script from A*Pathfinding. You can use it by simply including A*Pathfinding free
 * and uncommenting this code (and above edits on lines 5 and 11

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            smoothFollow.distance = 5;
            smoothFollow.height = 4;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            smoothFollow.distance = 15;
            smoothFollow.height = 10;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            smoothFollow.distance = 30;
            smoothFollow.height = 10;
        }
*/

    }
}
