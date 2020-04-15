using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Match {
    public class PlayerData {
        public byte PlayerId { get; private set; }
        public string Nickname { get; private set; }
        public bool IsBot { get; private set; }
        public int TeamIndex { get; private set; }
        public string CharacterId { get; private set; }

        public PlayerData(byte playerId, string nickname, bool isBot, int teamIndex, string characterId) {
            this.PlayerId = playerId;
            this.Nickname = nickname;
            this.IsBot = isBot;
            this.TeamIndex = teamIndex;
            this.CharacterId = characterId;
        }
    }
}
