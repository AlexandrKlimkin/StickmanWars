using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Controllers {
    public struct GamepadStatusChangedSignal {
        public string GamepadId;
        public GamepadStatus Status;
        public int Index;

        public GamepadStatusChangedSignal(string gamepadId, GamepadStatus status, int index) {
            this.GamepadId = gamepadId;
            this.Status = status;
            this.Index = index;
            Debug.Log($"{gamepadId} {status} {index}");
        }
    }

    public enum GamepadStatus {
        Connected,
        Disconnected,
        Reconnected
    }
}