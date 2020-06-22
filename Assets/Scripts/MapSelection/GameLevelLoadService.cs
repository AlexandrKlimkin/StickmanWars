using System.Collections;
using System.Collections.Generic;
using Core.Services;
using Core.Services.SceneManagement;
using Game.Match;
using KlimLib.SignalBus;
using KlimLib.Timers;
using UnityDI;
using UnityEngine;

namespace MapSelection {
    public class GameLevelLoadService : ILoadableService, IUnloadableService {

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly MatchData _MatchData;
        [Dependency]
        private readonly SceneManagerService _SceneManagerService;
        [Dependency]
        private readonly MapSelectionUIManager _MapSelectionUI;

        private const float LEVEL_LOAD_TIME = 4f;

        public void Load() {
            _SignalBus.Subscribe<LoadLvlTriggerInteractionSignal>(OnLoadLvlTriggerInteractionSignal, this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void OnLoadLvlTriggerInteractionSignal(LoadLvlTriggerInteractionSignal signal) {
            if (signal.TotalUnitsInsde < _MatchData.Players.Count) {
                StopCountDown();
            }
            else {
                StartCountDown(signal.SceneType);
            }
        }

        private void StartCountDown(SceneType scene) {
            _MapSelectionUI.LoadLevelTimer.gameObject.SetActive(true);
            _MapSelectionUI.LoadLevelTimer.StartDescendingTimer(LEVEL_LOAD_TIME, () => {
                _SceneManagerService.LoadScene(scene);
            });
        }

        private void StopCountDown() {
            _MapSelectionUI.LoadLevelTimer.StopTimer();
            _MapSelectionUI.LoadLevelTimer.gameObject.SetActive(false);
        }
    }
}
