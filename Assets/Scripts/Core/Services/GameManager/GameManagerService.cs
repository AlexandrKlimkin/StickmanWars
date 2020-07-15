using Core.Services.SceneManagement;
using Game.Match;
using KlimLib.SignalBus;
using UI;
using UI.Game;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class GameManagerService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly SceneManagerService _SceneManager;
        [Dependency]
        private readonly UIManager _UiManager;
        [Dependency]
        private readonly SignalBus _SignalBus;

        public bool GameInProgress { get; private set; }

        public void Load() {

        }

        public void Unload() {

        }

        public void StartMatch() {
            GameInProgress = true;
            _SignalBus.FireSignal(new MatchStartSignal());
        }

        public void EndMatch() {
            GameInProgress = false;
            _SignalBus.FireSignal(new MatchEndSignal());
            _UiManager.SetActivePanel<GameEndPanel>();
        }

        public void BackToMapSelection() {
            _SceneManager.LoadScene(SceneType.MapSelection);
        }

        public void RestartLevel() {

        }
    }
}
