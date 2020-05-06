using Core.Services.SceneManagement;
using Tools.Services;
using UI.Game;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class GameManagerService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly SceneManagerService _SceneManager;
        [Dependency]
        private readonly UIManager _UiManager;

        public bool GameInProgress { get; private set; }

        public void Load() {
            GameInProgress = true;
        }

        public void Unload() {

        }

        public void EndGame() {
            GameInProgress = false;
            _UiManager.SetActivePanel<GameEndPanel>();
        }

        public void RestartGame() {
            _SceneManager.LoadScene(SceneType.MapSelection);
        }
    }
}
