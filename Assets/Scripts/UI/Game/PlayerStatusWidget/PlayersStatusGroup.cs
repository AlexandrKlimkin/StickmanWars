using Core.Services.Game;
using Game.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace UI.Game {
    public class PlayersStatusGroup : MonoBehaviour {

        public List<PlayerStatusWidget> PlayerStatusWidgets;

        [Dependency]
        private readonly MatchData _MatchData;

        private void Awake() {
            ContainerHolder.Container.BuildUp(this);
            AssignWidgets();
        }

        private void AssignWidgets() {
            PlayerStatusWidgets.ForEach(_ => _.gameObject.SetActive(false));
            for(int i = 0; i < _MatchData.Players.Count; i++) {
                var player = _MatchData.Players[i];
                var widget = PlayerStatusWidgets[i];
                widget.gameObject.SetActive(true);
                widget.AssignToPlayer(player.PlayerId);
            }
        }
    }
}
