using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationAnchorSettings : MonoBehaviour {

	// Use this for initialization
	void Start () {

        CharacterController controller = GetComponent<CharacterController>();
        if(controller!=null)
        {
            controller.detectCollisions = false;

        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
