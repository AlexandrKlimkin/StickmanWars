using UnityEngine;
using System.Collections;
using System;

namespace Tools {
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

        public static T Instance { get; private set; }

        protected virtual void Awake() {
            RegisterSingleton();
        }

        protected virtual void OnEnable() {
            RegisterSingleton();
        }

        protected virtual void OnDestroy() {
            UnregisterSingleton();
        }

        protected void RegisterSingleton() {
            if (!Application.isPlaying || Instance == this)
                return;
            if (Instance == null) {
                Instance = this as T;
            }
            else {
                throw new Exception("Attempted to register second singleton instance of type " + Instance.GetType().Name);
            }
        }

        protected void UnregisterSingleton() {
            Instance = null;
        }
    }
}
