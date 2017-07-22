using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.t7t.formation;

namespace com.t7t.utilities
{

    // Source: http://wiki.unity3d.com/index.php/Toolbox
    // Under creative commons license: https://creativecommons.org/licenses/by-sa/3.0/


    public class Toolbox : Singleton<Toolbox>
    {
        protected Toolbox() { } // guarantee this will be always a singleton only - can't use the constructor!


        public List<FormationGrid> allFormations = new List<FormationGrid>();

        void Awake()
        {
            // Your initialization code here
        }


        public int GetFormationsCount()
        {
            return allFormations.Count;
        }

    }
}



