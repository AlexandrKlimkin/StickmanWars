using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Core.Services;
using KlimLib.SignalBus;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Services.Controllers {
    public class ControllersStatusService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly UnityEventProvider _EventProvider;

        public string[] Gamepads { get; private set; }
        private string[] _TempGamepads;

        public void Load() {
            _EventProvider.OnUpdate += CheckJoysticks;
            _TempGamepads = new string[0];
            CheckJoysticks();
        }

        public void Unload() { }

        private void CheckJoysticks() {
            Gamepads = Input.GetJoystickNames();
            var count = Gamepads.Length;
            var tempCount = _TempGamepads.Length;
            for (var i = 0; i < Gamepads.Length; i++) {
                var gamepad = Gamepads[i];
                if (tempCount - i > 0) {
                    var temp = _TempGamepads[i];
                    if (string.IsNullOrEmpty(temp)) {
                        if (!string.IsNullOrEmpty(gamepad))
                            _SignalBus.FireSignal(new GamepadStatusChangedSignal(gamepad, GamepadStatus.Reconnected, i + 1));
                    }
                    else {
                        if(string.IsNullOrEmpty(gamepad))
                            _SignalBus.FireSignal(new GamepadStatusChangedSignal(temp, GamepadStatus.Disconnected, i + 1));
                    }
                }
                else {
                    _SignalBus.FireSignal(new GamepadStatusChangedSignal(gamepad, GamepadStatus.Connected, i + 1));
                }
            }
            _TempGamepads = Gamepads.ToArray();
        }
    }
}
