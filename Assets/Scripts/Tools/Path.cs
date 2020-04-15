using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using UnityEngine;

public static class Path
{
    public static class Resources {
        public static string GameCamera => "Prefabs/Cameras/GameCamera";

        public static string CharacterPath(string characterId) {
            return $"Prefabs/Characters/{characterId}";
        }

        public static string Bone(string unitId, MuscleType type)
        {
            return $"Characters/{unitId}/{type}";
        }
    }
}