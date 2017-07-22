using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using com.t7t.utilities;
using UnityEngine.AI;

namespace com.t7t.formation
{
    [CustomEditor(typeof(FormationManager))]
    public class FormationManagerEditor : Editor
    {

        public FormationManager formationManager { get; private set; }

        [MenuItem("Window/Formations/Add Formation Manager", false, 1)]
        public static void AddFormationManager()
        {

            FormationManager exists = (FormationManager) FindObjectOfType<FormationManager>();
            if (!exists)
            {

                GameObject mngr = new GameObject("Formation Manager");

                if (mngr)
                {
                    mngr.transform.position = new Vector3(0, 0, 0);
                    mngr.AddComponent<FormationManager>();
                }
            }
            else Debug.LogWarning("A Formation Manager already exists in the Hierarchy");
        }


        public void OnEnable()
        {
            formationManager = target as FormationManager;
        }



        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            //serializedObject.Update();

            EditorGUILayout.HelpBox("Use below buttons to select whether you are using A*Pathfinding or (default) Unity Navmesh for navigation.", MessageType.Info);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("A*Pathfinding")) EnableAStar();
            if (GUILayout.Button("Unity Navmesh")) EnableNavMesh();
#if T7T_ASTAR
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Now using A*Pathfinding");
            EditorGUILayout.Space();
#else
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Now using Navmesh");
            EditorGUILayout.Space();
#endif
            GUILayout.EndHorizontal();


            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            //serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Create a new Anchor and Formation with below button. It will create the Formation and its Anchor in the middle of the Scene where on the plane/terrain/object. You can move and rename both afterwards. Don't forget select the formation type to add a target to the Anchor!", MessageType.Info);
            EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Create a new Formation:");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("New Formation")) CreateNewFormation();
            GUILayout.EndHorizontal();

        }

        private void EnableAStar()
        {
            // Add the AStar #define

            string currBuildSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            if (!currBuildSettings.Contains("T7T_ASTAR"))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currBuildSettings + ";T7T_ASTAR");
            }

        }

        private void EnableNavMesh()
        {
            // Remove the AStar #define

            string currBuildSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            if (currBuildSettings.Contains("T7T_ASTAR"))
            {
                string newBuildSettings = currBuildSettings.Replace(";T7T_ASTAR", "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newBuildSettings);
            }

        }

        public void CreateNewFormation()
        {

            /*
             * STEP 1: Define where to place the formation and how to name it
             * 
             * 
             */
 
            // Use a Ray / Raycast from the middle of the Scene View to place the formation and its anchor on the plane/terrain/object it hits
            //
            // http://answers.unity3d.com/questions/48979/how-can-i-find-the-world-position-of-the-center-of.html
            Ray worldRay = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));
            // http://josbalcaen.com/unity-editor-place-objects-by-raycast/
            Vector3 newPosition;
            RaycastHit hitInfo;
            // Shoot this ray. check in a distance of 10000.
            if (Physics.Raycast(worldRay, out hitInfo, 10000))
            {
                newPosition = hitInfo.point;
            }
            else newPosition = new Vector3(0, 0, 0);


            // We'll use the time to name the Formation and the Anchor
            float t = Time.time;
            // Time to string see: http://answers.unity3d.com/questions/45676/making-a-timer-0000-minutes-and-seconds.html

            /*
             * STEP 2: Create the Anchor 
             * 
             */

            GameObject anchor = new GameObject("Anchor " + string.Format("{0:0}:{1:00}.{2:0}",
                                            Mathf.Floor(t / 60),
                                            Mathf.Floor(t) % 60,
                                            Mathf.Floor((t * 10) % 10))
                                            );
            anchor.transform.position = newPosition; // new Vector3(0, 0, 0);
            anchor.transform.parent = formationManager.transform;

            FormationAnchor fa = anchor.AddComponent<FormationAnchor>();

#if T7T_ASTAR

#else
            // anchor.AddComponent<NavMeshAgent>();     No need to add since RequireComponent in FormationAnchor's class definition

            NavMeshAgent nma = anchor.GetComponent<NavMeshAgent>();
            if(nma!=null)
            {
                nma.radius = 0.1f;
                nma.height = 0.1f;

                nma.stoppingDistance = 1.0f;
            }


#endif
            
            GameObject sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere1.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            sphere1.transform.SetParent(anchor.transform);
            sphere1.transform.localPosition = new Vector3(0f, 0f, 0f);

            sphere1.transform.name = "empty";

            /*
             * STEP 3: Create the Formation and link the Anchor to it
             * 
             * 
             */

            GameObject formation = new GameObject("Formation " + string.Format("{0:0}:{1:00}.{2:0}", 
                                            Mathf.Floor(t / 60),
                                            Mathf.Floor(t) % 60,
                                            Mathf.Floor((t * 10) % 10))
                                            );
            formation.transform.position = newPosition;
            formation.transform.parent = formationManager.transform;

            FormationGrid fg = formation.AddComponent<FormationGrid>();

            AudioSource aud = formation.AddComponent<AudioSource>();

            GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere2.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            sphere2.transform.SetParent (formation.transform);
            sphere2.transform.localPosition = new Vector3(0f, 0f, 0f);
            sphere2.transform.name = "empty";

            //fg.ChangeGridTo(GridTypes.Wedge9);
            fg.SetAnchor(anchor);

        }


    }
}
