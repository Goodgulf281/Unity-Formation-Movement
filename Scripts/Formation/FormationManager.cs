using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.t7t.utilities;



namespace com.t7t.formation
{

    public class FormationManager : MonoBehaviour
    {


        public Toolbox toolbox;

        void Awake()
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

        }
    }

}
