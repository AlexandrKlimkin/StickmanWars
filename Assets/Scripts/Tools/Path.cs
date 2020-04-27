using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using UnityEngine;

public static class Path
{
    public static class Resources {
        public static string GameCamera => "Prefabs/Cameras/GameCamera";

        public static string MapSelectionUI => "Prefabs/UI/MapSelection/MapSelectionUI";
        public static string GameUI => "Prefabs/UI/Game/GameUI";

        public static string CharacterPath(string characterId) {
            return $"Prefabs/Characters/{characterId}";
        }

        public static string Bone(string unitId, MuscleType type)
        {
            return $"Characters/{unitId}/{type}";
        }
    }
}