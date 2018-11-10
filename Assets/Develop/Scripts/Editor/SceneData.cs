using System.Collections.Generic;
using UnityEngine;

namespace Assets.Editor
{
    public class SceneData : ScriptableObject
    {
        public List<Param> list = new List<Param>();

        [System.Serializable]
        public class Param
        {
            public int No;
            public string Name;
        }

    }
}
