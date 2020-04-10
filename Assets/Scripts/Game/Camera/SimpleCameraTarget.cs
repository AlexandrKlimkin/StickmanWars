using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Game.CameraTools
{
    public class SimpleCameraTarget : MonoBehaviour, ICameraTarget {
        [Dependency]
        private readonly SignalBus _SignalBus;

        public Vector3 Position => transform.position;
        public Vector3 Velocity => Vector3.zero;
        public float Direction => 0f;

        private void Start()
        {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, true));
            _SignalBus.Subscribe<GameCameraSpawnedSignal>(OnGameCameraSpawn, this);
        }

        private void OnDestroy()
        {
            _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, false));
        }

        private void OnGameCameraSpawn(GameCameraSpawnedSignal signal) {
            _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, true));
        }

    }
}