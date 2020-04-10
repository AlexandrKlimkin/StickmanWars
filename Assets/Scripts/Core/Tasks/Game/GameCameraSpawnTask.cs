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
        private readonly GameCameraSpawnSettings _CameraSpawnSettings;
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;
        [Dependency]
        private readonly SignalBus _SignalBus;

        protected override void AutoCompletedRun() {
            var camera = _ResourceLoader.LoadResourceOnScene<GameCameraBehaviour>(Path.Resources.GameCamera, _CameraSpawnSettings.SpawnTransform.position, _CameraSpawnSettings.SpawnTransform.rotation);
            ContainerHolder.Container.RegisterInstance(camera);
            ContainerHolder.Container.BuildUp(camera);
            _SignalBus.FireSignal(new GameCameraSpawnedSignal(camera));
        }
    }
}