using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using Core.Services.SceneManagement;
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
        [Dependency]
        private readonly SceneManagerService _SceneManagerService;

        protected override void AutoCompletedRun() {
            var activeScene = _SceneManagerService.ActiveScene;
            var cameraPath = Path.Resources.CameraPath(activeScene);
            var cameraBoundariesPath = Path.Resources.CameraBoundariesPath(activeScene);
            var camera = _ResourceLoader.LoadResourceOnScene<ProCamera2D>(cameraPath, _CameraSettings.SpawnTransform.position, _CameraSettings.SpawnTransform.rotation);
            ContainerHolder.Container.RegisterInstance(camera);
            var units = Object.FindObjectsOfType<CharacterUnit>();
            camera.AddCameraTargets(units.Select(_ => _.transform).ToList());
            var borders = _ResourceLoader.LoadResourceOnScene<ProCamera2DTriggerBoundaries>(cameraBoundariesPath);
            //ContainerHolder.Container.RegisterInstance(camera);
            //ContainerHolder.Container.BuildUp(camera);
            //camera.Initialize();
            //camera.SetBounds(_CameraSettings.CameraBounds);
            _SignalBus.FireSignal(new GameCameraSpawnedSignal(camera));
        }
    }
}