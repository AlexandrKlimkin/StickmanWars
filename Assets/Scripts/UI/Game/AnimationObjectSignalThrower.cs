using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace UI.Game {
    public class AnimationObjectSignalThrower : MonoBehaviour {
        public void ThrowSignal() {
            var signalBus = ContainerHolder.Container.Resolve<SignalBus>();
            signalBus.FireSignal(new AnimationObjectNameSignal(gameObject.name, transform.GetSiblingIndex()));
        }
    }

    public struct AnimationObjectNameSignal {
        public string ObjectName;
        public int Index;

        public AnimationObjectNameSignal(string objectName, int index) {
            this.ObjectName = objectName;
            this.Index = index;
        }
    }
}
