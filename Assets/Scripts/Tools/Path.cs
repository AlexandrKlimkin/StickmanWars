using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using UnityEngine;

public static class Path
{
    public static class Resources
    {

        public static string Bone(string unitId, MuscleType type)
        {
            return $"Characters/{unitId}/{type}";
        }
    }
}