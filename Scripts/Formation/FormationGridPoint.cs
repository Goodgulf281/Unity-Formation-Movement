using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.t7t.formation
{

    /**
     * This contains the details for a single grid point in the FormationGrid including the assigned unit (GameObject), a sphere used for visualization
     * of the grid point and cached components. Cached components include the Character controller used for moving the assigned unit towards the grid point coordinates
     * and the FormationUnitAnimation which is used the animate the unit when moving.
     * 
     * Key properties: position, assignedUnit, offsetX, offsetZ, sphere.
     * 
     * Key public methods: FormationGridPoint [constructor], AssignUnit, SetPosition
     * 
     * 
     */


    public class FormationGridPoint
    {

        public int id;                                              // identifies this grid point and enabled units to be reassigned when the FormationGrid's GridType changes

        protected Vector3 position;                                 // the position on the map (world coordinates) where this FormationGridPoint is located

        protected GameObject assignedUnit;                          // Unit in the formation assigned to this grid point
        protected Transform trUnit;                                 // Assigned Unit's cached Transform 
        protected CharacterController unitController;               // Assigned Unit's cached Character Controller (if present)
        protected Rigidbody unitRigidbody;                          // Assigned Unit's cached Rigid Body (if present)
        protected Animator unitAnim;                                // Assigned Unit's cached animator
        protected FormationUnitAnimation formationUnitAnimation;    // Assigned Unit's cached FormationUnitAnimation which calculates the actual required animation states/variables
        protected float distanceToUnit;                             // distance from assigned unit to this grid point

        protected Vector3 assignedVelocity;                         // velocity assigned to the assigned unit's character controller in the FormationGrid Update function.

        protected float unitVerticalSpeed = 0.0f;                   // Unit's vertical speed used for gravity

        public float offsetX;                                       // (local) offset to the right from the FormationAnchor position in the FormationGrid.
        public float offsetZ;                                       // (local) offset to the front from the FormationAnchor position in the FormationGrid.
        public float distanceToGround = 0.0F;

        protected float randomOffset = 0.0F;                        // if >0 then the offsets are randomized with this distance
        protected float randomOffsetX = 0.0F;
        protected float randomOffsetZ = 0.0F;
        protected float oldrandomOffsetX = 0.0F;
        protected float oldrandomOffsetZ = 0.0F;
        protected float timetophaseout = 0.0F;

        protected Vector3 disbandDestination;

        private GameObject sphere;                                  // this sphere is used to visualize the FormationGrid if that flag is checked (in FormationGrid)


        public FormationGridPoint(int identifier, GameObject parent, float randomoffset)
        {
            id = identifier;

            randomOffset = randomoffset;

            // Create the sphere used to visualize the grid and attach it to the FormationGrid in the hierarchy.
            // Sphere color red signifies the FormationGridPoint has no unit assigned to it. It turns green if a unit has been assigned.
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            sphere.transform.parent = parent.transform;
            sphere.transform.name = "GridPoint " + identifier;
            SetSphereColor(Color.red);

            // Remove the sphere's collider to ensure the assigned unit is not blocked by it or moves over/around it:
            SphereCollider collider = sphere.GetComponent<SphereCollider>();
            if (collider)
            {
                collider.enabled = false;
            }

            sphere.active = false;
        }

        // Change the sphere's color to show it's state (Red = no unit assigned, Green = unit assigned):
        protected void SetSphereColor (Color color)
        {
            Renderer renderer = sphere.GetComponent<Renderer>();
            if (renderer)
            {
                renderer.material.color = color;
            }
        }

        public void DestroySphere()
        {
            Object.Destroy(sphere);
        }

        // These public GetX functions are used to retreive the values for the protected properties
        #region Getters
        public Vector3 GetPosition()
        {
            return position;
        }

        public Transform GetTransform()
        {
            return sphere.transform;
        }

        public GameObject GetAssignedUnit()
        {
            return assignedUnit;
        }

        public CharacterController GetCharacterController()
        {
            return unitController;
        }

        public Rigidbody GetRigidbody()
        {
            return unitRigidbody;
        }

        public Animator GetUnitAnimator()
        {
            return unitAnim;
        }

        public FormationUnitAnimation GetFormationUnitAnimation()
        {
            return formationUnitAnimation;
        }

        public float GetUnitVerticalSpeed()
        {
            return unitVerticalSpeed;
        }

        public void SetUnitVerticalSpeed(float speed)
        {
            unitVerticalSpeed = speed;
        }

        public void SetAssignedVelocity (Vector3 vlcty)
        {
            assignedVelocity = vlcty;
        }
        public Vector3 GetAssignedVelocity ()
        {
            return assignedVelocity;
        }

        #endregion


        // Assign the GameObject which will be following this FormationGridPoint.
        // Cache the unit's components: unit's transform (mandatory), CharacterController (mandatory), Animator (optional), FormationUnitAnimation (optional)
        public void AssignUnit(GameObject unit)
        {
            if (unit != null)
            {
                assignedUnit = unit;
                trUnit = unit.transform;

                
                unitController = unit.GetComponent<CharacterController>();
                /*if (unitController == null)
                {
                    Debug.LogError("FormationGridPoint.AssignUnit(): unit has no CharacterController");
                }*/

                unitRigidbody = unit.GetComponent<Rigidbody>();


                unitAnim = unit.GetComponent<Animator>();
                if (unitAnim == null)
                {
                    Debug.LogWarning("FormationGridPoint.AssignUnit(): unit has no Animator");
                }

                formationUnitAnimation = unit.GetComponent<FormationUnitAnimation>();
                if (formationUnitAnimation == null)
                {
                    Debug.LogWarning("FormationGridPoint.AssignUnit(): unit has no FormationUnitAnimation");
                }

                SetSphereColor(Color.green);
            }
        }

        public bool IsUnitAssigned()
        {
            return (assignedUnit!=null);
        }

        // Calculates the distance between the assigned unit's position and the FormationGridPoint's position and return the value:
        public float CalculateDistanceUnitToGridPoint()
        {
            //distanceToUnit = (position - assignedUnit.transform.position).magnitude;
            CalculateDistanceFromGridPoint();

            return distanceToUnit;
        }

        // Calculates the distance between the assigned unit's position and the FormationGridPoint's position and only store the value:
        public void CalculateDistanceFromGridPoint()
        {
            if (assignedUnit != null)
            {
                distanceToUnit = Vector3.Distance(trUnit.position, position);
            }
            else distanceToUnit = -1.0F;    // Returns -1 when error
        }


        public void RandomizePosition()
        {
            if (randomOffset > 0.0F)
            {
                oldrandomOffsetX = randomOffsetX;
                oldrandomOffsetZ = randomOffsetZ;
                timetophaseout = 0.0F;

                Vector2 position = Random.insideUnitCircle * randomOffset;
                randomOffsetX = position.x;
                randomOffsetZ = position.y;
            }
            else {
                randomOffsetX = 0.0F;
                randomOffsetZ = 0.0F;
            };
        }


        // Sets the position based on:
        //  pos   = position of the FormationAnchor
        //  rotY  = the rotation of the FormationGrid along its y-axis
        //  scale = scale property of the FormationGrid which stretches the grid in both x and z direction
        //  mask  = used in the Physics.Raycast to mask the terrain hit and determine the ground level of (typically) the terrain

        // Update 2017-07-8: Added the random offsets X and Z to randomize the positions slightly.

        public void SetPosition(Vector3 pos, float rotY, float scale, LayerMask mask)
        {
            // calculate the local position

            timetophaseout += Time.deltaTime;
            if (timetophaseout > 1.0F)
                timetophaseout = 1.0F;

            Vector2 originalOffset = new Vector2(oldrandomOffsetX, oldrandomOffsetZ);
            Vector2 newOffset = new Vector2(randomOffsetX, randomOffsetZ);

            Vector2 calcOffset = Vector2.Lerp(originalOffset, newOffset, timetophaseout);


            Vector3 newPosition = new Vector3((offsetX+calcOffset.x) * scale, distanceToGround, (offsetZ+calcOffset.y) * scale);
            newPosition = Quaternion.Euler(0, rotY, 0) * newPosition;
            // add the world position
            newPosition += pos;

            // calculate height:
            Vector3 rayPosition = newPosition;
            // assume +100 covers the maxium height range within the FormationGrid bounds.
            rayPosition.y += 100;
            RaycastHit hit;
            if (Physics.Raycast(rayPosition, -Vector3.up, out hit, Mathf.Infinity, mask))
            {
                newPosition.y = hit.point.y+distanceToGround;
                Debug.DrawLine(newPosition, hit.point, Color.cyan);
            }

            // set the actual position of the FormationGridPoint based on above calculations:
            SetPosition(newPosition);
        }

        public void SetPosition(Vector3 pos)
        {
            position = pos;

            if(sphere)
            {
                sphere.transform.position = pos;
            }
        }

        public void SetPositionToDisband(LayerMask mask)
        {
            SetPosition(disbandDestination, 0, 1, mask);
        }

        public void SetDisbandDesitination(float range, LayerMask mask)
        {
            disbandDestination = position + Random.insideUnitSphere * range;

            Vector3 rayPosition = disbandDestination;
            // assume +100 covers the maxium height range within the UnitSphere*range bounds.
            rayPosition.y += 100;
            RaycastHit hit;
            if (Physics.Raycast(rayPosition, -Vector3.up, out hit, Mathf.Infinity, mask))
            {
                disbandDestination.y = hit.point.y + distanceToGround;

                DebugPanel.Log("disband dest for "+this.id,"Disband",disbandDestination);
                //Debug.DrawLine(disbandDestination, position, Color.green);
            }

        }

        public void VisualizeGridPoint(bool show)
        {
            sphere.active = show;
        }
    }

}
