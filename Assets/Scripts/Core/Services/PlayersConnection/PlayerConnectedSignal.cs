using Game.Match;
using InControl;

namespace Core.Services.Game {
    public struct PlayerConnectedSignal {
        public PlayerData PlayerData;
        public bool IsLocalplayer;
        public PlayerActions PlayerActions;

        public PlayerConnectedSignal(PlayerData playerData, bool isLocalplayer, PlayerActions playerActions) {
            this.PlayerData = playerData;
            this.IsLocalplayer = isLocalplayer;
            this.PlayerActions = playerActions;
        }
    }
}
