using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Json
{
    [Serializable]
    public class JsonObject
    {
        public List<Situation> situations;

        public static List<Situation> GetSituations()
        {
            using (var streamReader = new StreamReader("Assets/Scripts/Data/situations.json"))
            {
                var json = streamReader.ReadToEnd();
                streamReader.Close();
            
                return JsonUtility.FromJson<JsonObject>(json).situations;
            }
        }
    }
}