using Game.Match;

namespace Core.Services.Game {
    public struct PlayerConnectedSignal {
        public PlayerData PlayerData;
        public bool IsLocalplayer;
        public int DeviceId;

        public PlayerConnectedSignal(PlayerData playerData, bool isLocalplayer, int deviceId) {
            this.PlayerData = playerData;
            this.IsLocalplayer = isLocalplayer;
            this.DeviceId = deviceId;
        }
    }
}
