using Game.Match;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class LocalPlayersConnectionProcessor : PlayersConnectionProcessor {

        public override void ProcessConnection() {

        }

        private void TryToAddPlayer(KeyCode keyCode, int deviceIndex) {
            if (Input.GetKeyDown(keyCode)) {
                PlayerConnected?.Invoke(deviceIndex);
            }
        }
    }
}
