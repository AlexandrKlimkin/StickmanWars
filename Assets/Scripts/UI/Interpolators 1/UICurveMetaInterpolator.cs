using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI {
    public class UICurveMetaInterpolator : UIMetaInterpolator {
        [SerializeField]
        private AnimationCurve _Curve = AnimationCurve.EaseInOut(0,0,1,1);

        protected override float ProcessFraction(float fraction) {
            return base.ProcessFraction(_Curve.Evaluate(fraction));
        }
    }
}