using System;
using UnityEngine;
using Tools;

namespace Tools.Unity {
    public class UnityEventProvider : MonoBehaviour {
        public event Action OnUpdate = () => { };
        public event Action OnFixedUpdate = () => { };
        public event Action<bool> OnAppPause = _ => { };
        public event Action OnAppQuit = () => { };

        private void Update() {
            OnUpdate.Invoke();
        }

        private void FixedUpdate() {
            OnFixedUpdate.Invoke();
        }

        private void OnApplicationPause(bool pause) {
            OnAppPause.Invoke(pause);
        }

        private void OnApplicationQuit() {
            OnAppQuit.Invoke();
        }
    }
}