using Core.Services.SceneManagement;
using Game.Match;
using KlimLib.SignalBus;
using System.Linq;
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
        [Dependency]
        private readonly MatchService _MatchService;
        [Dependency]
        private readonly MatchData _MatchData;

        public bool GameInProgress { get; private set; }

        public void Load() {
            ContainerHolder.Container.BuildUp(this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        public void StartMatch() {
            ContainerHolder.Container.BuildUp(this); //ToDo: remove
            GameInProgress = true;
            _SignalBus.FireSignal(new MatchStartSignal());
            _UiManager.SetActivePanel<MainPanel>();
        }

        public void EndMatch() {
            ContainerHolder.Container.BuildUp(this); //ToDo: remove
            GameInProgress = false;
            _SignalBus.FireSignal(new MatchEndSignal());
            _UiManager.SetActivePanel<GameEndPanel>();
        }

        public void BackToMapSelection() {
            _SceneManager.LoadScene(SceneType.MapSelection);
        }

        public void RestartLevel() {

        }

        public void AddPlayerOnMap(string characterId, bool bot) {
            byte playerId = 0;
            if (_MatchData.Players.Count > 0) {
                playerId = _MatchData.Players.Max(_ => _.PlayerId);
                playerId++;
            }
            var player = new PlayerData(playerId, characterId, bot, playerId, characterId);
            _MatchService.AddPlayer(player);
        }
    }
}
