using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Core.Services;
using InControl;
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

        private List<InputDevice> _InputDevices = new List<InputDevice>();
        public IReadOnlyList<InputDevice> InputDevices => _InputDevices;
        public InputDevice InputDevice => InputManager.ActiveDevice;

        private string _SaveData;
        //private PlayerActions _PlayerActions;

        public void Load() {
            _EventProvider.OnUpdate += CheckInputDevices;
            CheckInputDevices();
            //CreateInputBindings();
        }

        public void Unload() {
            //SaveBindings();
            //_PlayerActions.Destroy();
        }

        //private void CreateInputBindings() {
        //    _PlayerActions = PlayerActions.CreateWithDefaultBindings();
        //    LoadBindings();
        //}

        //private void SaveBindings() {
        //    _SaveData = _PlayerActions.Save();
        //    PlayerPrefs.SetString("Bindings", _SaveData);
        //}

        //private void LoadBindings() {
        //    if (PlayerPrefs.HasKey("Bindings")) {
        //        _SaveData = PlayerPrefs.GetString("Bindings");
        //        _PlayerActions.Load(_SaveData);
        //    }
        //}

        private void CheckInputDevices() {
            var inputDevice = InputManager.ActiveDevice;
            if (!_InputDevices.Contains(inputDevice)) {
                _InputDevices.Add(inputDevice);
                Debug.Log($"Input device added {inputDevice.Name}, All devices count = {_InputDevices.Count})");
            }
        }
    }
}
