using UnityEngine;

namespace Character.Control {
    public abstract class AimViewBase : MonoBehaviour {

        protected IAimProvider _Provider;
        protected Transform _AimStartTransform;

        public void Setup(IAimProvider provider, Transform aimStartTransform) {
            _Provider = provider;
            _AimStartTransform = aimStartTransform;
        }
    }
}
