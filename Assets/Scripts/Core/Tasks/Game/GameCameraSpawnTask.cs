using System.Collections;
using System.Collections.Generic;
using Game.CameraTools;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using KlimLib.TaskQueueLib;
using Rendering;
using UnityDI;
using UnityEngine;

namespace Core.Initialization.Game {
    public class GameCameraSpawnTask : AutoCompletedTask {
        [Dependency]
        private readonly GameCameraSettings _CameraSettings;
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;
        [Dependency]
        private readonly SignalBus _SignalBus;

        protected override void AutoCompletedRun() {
            var camera = _ResourceLoader.LoadResourceOnScene<GameCameraBehaviour>(Path.Resources.GameCamera, _CameraSettings.SpawnTransform.position, _CameraSettings.SpawnTransform.rotation);
            ContainerHolder.Container.RegisterInstance(camera);
            ContainerHolder.Container.BuildUp(camera);
            camera.SetBounds(_CameraSettings.CameraBounds);
            _SignalBus.FireSignal(new GameCameraSpawnedSignal(camera));
        }
    }
}