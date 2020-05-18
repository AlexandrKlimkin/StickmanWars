using System;
using System.Collections;
using System.Collections.Generic;
using Core.Services;
using KlimLib.TaskQueueLib;
using Tools.Unity;
using UnityDI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.SceneManagement {
    public class SceneManagerService : ILoadableService {

        [Dependency]
        private readonly UnityEventProvider _EventProvider;

        private readonly Dictionary<SceneType, SceneLoadingParameters> _SceneLoadingParametersMap = new Dictionary<SceneType, SceneLoadingParameters>() {
            { SceneType.MapSelection, new MapSelectionLoadingParameters() },
            { SceneType.CityCrane, new GameLoadingParameters() },
        };

        public SceneType ActiveScene {
            get {
                Enum.TryParse(SceneManager.GetActiveScene().name, false, out SceneType result);
                return result;
            }
        }

        public bool IsGameScene {
            get {
                var scene = ActiveScene;
                return ActiveScene == SceneType.CityCrane;
            }
        }

        public void Load() {

        }

        public void LoadScene(SceneType scene) {
            var activeScene = ActiveScene;
            if (activeScene == scene)
                return;
            var oldSceneParameters = _SceneLoadingParametersMap[activeScene];
            var newParameters = _SceneLoadingParametersMap[scene];
            oldSceneParameters.BeforeUnload();
            oldSceneParameters.UnloadingTasks.RunTasksListAsQueue(
                () => {
                    OnSceneUnloadSuccess(activeScene);
                    oldSceneParameters.AfterUnload();
                    newParameters.BeforeLoad();
                    SceneManager.LoadScene(scene.ToString());
                    newParameters.LoadingTasks.RunTasksListAsQueue(
                        () => {
                            OnSceneLoadSuccess(scene);
                            newParameters.AfterLoad();
                        },
                        (task, e) => {
                            OnSceneLoadFail(scene);
                            Debug.LogError($"{task} task failed with {e}");
                        },
                        null);
                },
                (task, e) => {
                    OnSceneUnloadFail(scene);
                    Debug.LogError($"{task} task failed with {e}");
                },
                null);
        }

        private void OnSceneUnloadSuccess(SceneType scene) {
            Debug.Log($"{scene} successfully unloaded");
        }
        private void OnSceneUnloadFail(SceneType scene) {
            Debug.LogError($"{scene} unload fail");
        }
        private void OnSceneLoadSuccess(SceneType scene) {
            Debug.Log($"{scene} successfully loaded");
        }
        private void OnSceneLoadFail(SceneType scene) {
            Debug.LogError($"{scene} load fail");
        }
    }

    public enum SceneType {
        MapSelection,
        CityCrane,
    }
}
