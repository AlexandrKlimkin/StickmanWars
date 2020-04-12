using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match {
    public class PlayerData {

        public byte PlayerId { get; private set; }
        public string Nickname { get; private set; }
        public bool IsBot { get; private set; }
        public int TeamIndex { get; private set; }

        public PlayerData(byte playerId) {
            this.PlayerId = playerId;
        }
    }
}
