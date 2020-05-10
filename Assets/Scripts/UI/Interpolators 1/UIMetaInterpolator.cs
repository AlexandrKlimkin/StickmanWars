using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEngine.UI {
    public abstract class UIMetaInterpolator : UIInterpolator {
        [SerializeField]
        protected List<UIInterpolator> _Interpolators;

        [Button("GatherInterpolators")]
        [SerializeField]
        private bool _GatherInterpolators;

        protected virtual float ProcessFraction(float fraction) {
            return Mathf.Clamp01(fraction);
        }

        protected sealed override void OnSetInterpolationFraction(float fraction) {
            var processed = ProcessFraction(fraction);
            _Interpolators.ForEach(_ => _.SetFraction(processed));
        }

        public void GatherInterpolators() {
            var childInterpolators = this.GetComponentsInChildren<UIInterpolator>();
            if (!_Interpolators.IsNullOrEmpty()) {
                _Interpolators = _Interpolators
                    .Concat(childInterpolators)
                    .Distinct()
                    .Where(_ => _ != null && _ != this)
                    .ToList();
            }
            else {
                _Interpolators = childInterpolators.Where(_ => _ != this).ToList();
            }
        }
    }
}