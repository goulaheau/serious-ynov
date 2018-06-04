using System;
using System.Collections.Generic;
using UnityEngine;

namespace Json
{
    [Serializable]
    public class Situation
    {
        public string description;
        public Vector3 position;
        public Vector3 rotation;
        public List<Choice> choices;
    }
}