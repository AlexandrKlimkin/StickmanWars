using System.Collections;
using System.Collections.Generic;
using Core.Services.Game;
using Game.Match;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace UI.Game {
    public class GameEndPanel : UIPanel {
        [Dependency]
        private readonly GameManagerService _GameManagerService;
        [Dependency]
        private readonly MatchService _MatchService;
        [Dependency]
        private readonly MatchData _MatchData;
        [Dependency]
        private readonly IPlayerLifesCounter _PlayerLifesCounter;
        [Dependency]
        private readonly BattleStatisticsService _BattleStatisticsService;

        public Button BackToMapSelectionButton;
        public Button RestartButton;

        public List<RatingPlaceWidget> _RatingPlaceWidgets;

        private void Start() {
            BackToMapSelectionButton.onClick.AddListener(BackToMapSelection);
            RestartButton.onClick.AddListener(Restart);
        }

        public override void Setup() {
            base.Setup();
            RefreshGameEndStats();
        }

        private void Restart() {

        }

        private void BackToMapSelection() {
            _GameManagerService.BackToMapSelection();
        }

        private void RefreshGameEndStats() {
            var playersRatingList = _PlayerLifesCounter.PlayersLifesDict;
            var sortedPlayers = _PlayerLifesCounter.PlayersLifesDict.OrderByDescending(_=>_.Value).Select(_=>_.Key).ToList();
            sortedPlayers.OrderByDescending(_=> _BattleStatisticsService.KillsDict[_].Count);

            for(int i = 0; i < _RatingPlaceWidgets.Count; i++) {
                var widget = _RatingPlaceWidgets[i];
                widget.gameObject.SetActive(i < sortedPlayers.Count);
            }
            for(int i = 0; i < sortedPlayers.Count; i++) {
                var player = sortedPlayers[i];
                var widget = _RatingPlaceWidgets[i];
                widget.DisplayStats(player, i + 1);
            }
        }
    }
}
