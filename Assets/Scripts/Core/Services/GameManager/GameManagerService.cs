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

        public void AddPlayerOnMap(string characterId, bool bot) {
            var lastPlayerId = _MatchData.Players.Max(_=>_.PlayerId);
            lastPlayerId++;
            var player = new PlayerData(lastPlayerId, characterId, bot, lastPlayerId, characterId);
            _MatchService.AddPlayer(player);
        }
    }
}
