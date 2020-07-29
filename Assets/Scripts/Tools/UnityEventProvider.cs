using System;
using UnityEngine;
using Tools;

namespace Tools.Unity {
    public class UnityEventProvider : MonoBehaviour {
        public event Action OnUpdate = () => { };
        public event Action OnFixedUpdate = () => { };
        public event Action OnLateUpdate = () => { };
        public event Action<bool> OnAppPause = _ => { };
        public event Action OnAppQuit = () => { };
        public event Action OnGizmos = () => { };

        private void Awake() {
            DontDestroyOnLoad(this);
        }

        private void Update() {
            OnUpdate.Invoke();
        }

        private void FixedUpdate() {
            OnFixedUpdate.Invoke();
        }

        private void LateUpdate() {
            OnLateUpdate.Invoke();
        }

        private void OnApplicationPause(bool pause) {
            OnAppPause.Invoke(pause);
        }

        private void OnApplicationQuit() {
            OnAppQuit.Invoke();
        }

        private void OnDrawGizmos() {
            OnGizmos.Invoke();
        }
    }
}