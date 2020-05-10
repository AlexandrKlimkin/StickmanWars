using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using System.Linq;

namespace UnityEngine.UI {
  
    public class UIInterpolatorGroup : UIInterpolator {

        public List<UIInterpolator> Interpolators;

        //[Range(0, 1)]
        //[SerializeField]
        //protected float _Fraction;

        [Button("GatherInterpolators")]
        [SerializeField]
        private bool _GatherInterpolators;


//#if UNITY_EDITOR
//        protected override void OnValidate() {
//            base.OnValidate();
//            SetInterpolationFraction(_Fraction);
//        }
//#endif

        //public void SetFraction(float fraction) {
        //    _Fraction = fraction;
        //    SetInterpolationFraction(_Fraction);
        //}

        public void GatherInterpolators() {
            var childInterpolators = this.GetComponentsInChildren<UIInterpolator>(true);
            if (!Interpolators.IsNullOrEmpty()) {
                Interpolators = Interpolators
                    .Concat(childInterpolators)
                    .Distinct()
                    .Where(_ => _ != null && _ != this)
                    .ToList();
            }
            else {
                Interpolators = childInterpolators.Where(_=>_!=this).ToList();
            }
        }

        protected override void OnSetInterpolationFraction(float fraction) {
            if (Interpolators.IsNullOrEmpty())
                return;
            Interpolators.ForEach(_ => {
                if (_ != null)
                    _.SetFraction(fraction);
            });
        }
    }
}
