using UnityDI;
using UnityEngine;

namespace Game.CameraTools {

    public class MainCamera : MonoBehaviour {

        public Camera Camera { get; private set; }

        private void Awake() {
            ContainerHolder.Container.RegisterInstance(this);
            Camera = this.GetComponentInChildren<Camera>();
        }

        private void OnDestroy() {
            ContainerHolder.Container.Unregister<MainCamera>();
        }
    }
}