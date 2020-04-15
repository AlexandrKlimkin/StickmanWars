using System;

namespace Core.Services.Game {
    public abstract class PlayersConnectionProcessor {
        public Action<int> PlayerConnected; // int - joystickNum
        public abstract void ProcessConnection();
    }
}