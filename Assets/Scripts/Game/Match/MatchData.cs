using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match {
    public class MatchData {
        public List<PlayerData> Players;

        public MatchData(List<PlayerData> Players) {
            this.Players = Players;
        }
    }
}
