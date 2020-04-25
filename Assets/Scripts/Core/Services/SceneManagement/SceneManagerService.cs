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

        private Dictionary<SceneType, SceneLoadingParameters> _SceneLoadingParametersMap = new Dictionary<SceneType, SceneLoadingParameters>() {
            { SceneType.MapSelection, new MapSelectionLoadingParameters() },
            { SceneType.CityCrane, new GameLoadingParameters() },
        };

        public SceneType ActiveScene {
            get {
                Enum.TryParse(SceneManager.GetActiveScene().name, false, out SceneType result);
                return result;
            }
        }

        public void Load() {

        }

        public void LoadScene(SceneType scene) {
            var activeScene = ActiveScene;
            if (activeScene == scene)
                return;
            _EventProvider.StartCoroutine(LoadSceneRoutine(scene, activeScene));
        }

        private IEnumerator LoadSceneRoutine(SceneType newScene, SceneType currentScene) {
            var oldSceneParameters = _SceneLoadingParametersMap[currentScene];
            var newParameters = _SceneLoadingParametersMap[newScene];
            oldSceneParameters.BeforeUnload();
            oldSceneParameters.UnloadingTasks.RunTasksListAsQueue(
                () => {
                    OnSceneUnloadSuccess(currentScene);
                    oldSceneParameters.AfterUnload();
                },
                (task, e) => {
                    OnSceneUnloadFail(newScene);
                    Debug.LogError($"{task} task failed with {e}");
                },
                null);
            newParameters.BeforeLoad();
            SceneManager.LoadScene(newScene.ToString());
            yield return null;
            yield return null;
            newParameters.LoadingTasks.RunTasksListAsQueue(
                () => {
                    OnSceneLoadSuccess(newScene);
                    newParameters.AfterLoad();
                },
                (task, e) => {
                    OnSceneLoadFail(newScene);
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
