using Com.LuisPedroFonseca.ProCamera2D;
using Game.CameraTools;

namespace Game.CameraTools {
    public struct GameCameraSpawnedSignal {
        public ProCamera2D Camera;

        public GameCameraSpawnedSignal(ProCamera2D camera) {
            this.Camera = camera;
        }
    }
}