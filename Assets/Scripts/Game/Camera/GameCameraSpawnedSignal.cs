using Game.CameraTools;

namespace Game.CameraTools {
    public struct GameCameraSpawnedSignal {
        public GameCameraBehaviour Camera;

        public GameCameraSpawnedSignal(GameCameraBehaviour camera) {
            this.Camera = camera;
        }
    }
}