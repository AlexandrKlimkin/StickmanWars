using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI {
    [RequireComponent(typeof(UIInterpolatorGroup))]
    [AddComponentMenu("Layout/UI Curve Interpolator Driver", 102)]
    public class UICurveInterpolationDriver : UIInterpolatorDriver {
        [SerializeField]
        private float _TransitioTime = 0.5f;
        [SerializeField]
        private AnimationCurve _Curve = AnimationCurve.Linear(0,0,1,1);

        private float _CurvePosition;
        private long lastframe=-1;

        protected override float GetNextFraction() {
            if (Time.frameCount != lastframe) {
                _CurvePosition = Mathf.MoveTowards(_CurvePosition, TargetFraction, _AnimationTimeScale * TimeStep() / _TransitioTime);
                lastframe = Time.frameCount;
            } 
            return _Curve.Evaluate(_CurvePosition);
        }
        protected override void ProcessSnap() {
            base.ProcessSnap();
            _CurvePosition = TargetFraction;
        }

        protected override bool CheckTransitionComplete() {
            return Mathf.Abs(_CurvePosition - TargetFraction) < Epsylon;
        }

        private float TimeStep() {
            return Mathf.Min(DeltaTime, 1/30f);
        }
    }
}