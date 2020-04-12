using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match {
    public class MatchData {
        public IReadOnlyList<PlayerData> Players => _Players;
        private List<PlayerData> _Players;

        public MatchData(List<PlayerData> Players) {
            this._Players = Players;
        }
    }
}
