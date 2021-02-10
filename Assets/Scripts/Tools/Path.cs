using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using Core.Services.SceneManagement;
using UnityEngine;

public static class Path {
    public static class Resources {
        //public static string MapSelectionCamera => "Prefabs/Cameras/MapSelectionCamera";
        //public static string MapSelectionCameraBoundaries => "Prefabs/Cameras/MapSelectionCameraBoundaries";

        //public static string GameCamera => "Prefabs/Cameras/GameCamera";
        //public static string GameCameraBoundaries => "Prefabs/Cameras/GameCameraBoundaries";

        public static string MapSelectionUI => "Prefabs/UI/MapSelection/MapSelectionUI";
        public static string GameUI => "Prefabs/UI/Game/GameUI";

        public static string CharacterPath(string characterId) {
            return $"Prefabs/Characters/{characterId}";
        }

        public static string RagdollPath(string characterId) {
            return $"Prefabs/Characters/Ragdolls/{characterId}_ragdoll";
        }

        public static string Bone(string unitId, MuscleType type) {
            return $"Characters/{unitId}/{type}";
        }

        public static string WeaponPreview(string weaponId) {
            return $"Previews/Items/{weaponId}";
        }

        public static string CameraPath(SceneType sceneType) {
            return $"Prefabs/Cameras/{sceneType.ToString()}Camera";
        }

        public static string CameraBoundariesPath(SceneType sceneType) {
            return $"Prefabs/Cameras/{sceneType.ToString()}CameraBoundaries";
        }
    }
}