using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

namespace UnityEngine.UI {
    [RequireComponent(typeof(UIInterpolatorGroup))]
    [AddComponentMenu("Layout/UI Damping Interpolator Driver", 102)]
    public class UIDampingInterpolatorDriver : UIInterpolatorDriver {
        [SerializeField]
        private bool _Symmetrical;

        [SerializeField]
        private bool _Inverted;

        private bool _UseInvertedDist => (!(CurrentFraction < TargetFraction) && _Symmetrical) ^ _Inverted;

        public float Damping = 7;

        protected override float GetNextFraction() {
            var dist = Mathf.Abs(TargetFraction - CurrentFraction);

            return DistanceDrivenLerp(_UseInvertedDist ? 1 - dist : dist);
        }

        private float DistanceDrivenLerp(float param) {

            return Mathf.MoveTowards(CurrentFraction, TargetFraction, Mathf.Max(Epsylon, _AnimationTimeScale * DeltaTime * Damping * param));
        }
    }
}
