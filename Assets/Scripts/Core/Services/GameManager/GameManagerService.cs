using Core.Services.SceneManagement;
using Game.Match;
using KlimLib.SignalBus;
using System.Collections;
using System.Linq;
using Tools.Unity;
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
        [Dependency]
        private readonly UnityEventProvider _EventProvider;

        public static bool GameInProgress { get; private set; }
        public static bool MatchStarted { get; private set; }

        public void Load() {
            ContainerHolder.Container.BuildUp(this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
            _EventProvider.StopCoroutine(FightSignalRoutine());
            GameInProgress = false;
            MatchStarted = false;
        }

        public void StartMatch() {
            ContainerHolder.Container.BuildUp(this); //ToDo: remove
            GameInProgress = true;
            MatchStarted = false;
            _SignalBus.FireSignal(new MatchReadySignal());
            _UiManager.SetActivePanel<MainPanel>();
            _EventProvider.StartCoroutine(FightSignalRoutine());
        }

        private IEnumerator FightSignalRoutine() {
            yield return new WaitForSeconds(5.5f);
            MatchStarted = true;
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
