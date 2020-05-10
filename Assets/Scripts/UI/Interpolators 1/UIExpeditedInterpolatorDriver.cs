using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI {
    [RequireComponent(typeof(UIInterpolatorGroup))]
    [AddComponentMenu("Layout/UI FastDamping Interpolator Driver", 102)]
    public class UIExpeditedInterpolatorDriver : UIDampingInterpolatorDriver {
        [SerializeField]
        private float _Cutoff = 0.1f;

        protected override float Epsylon => TargetFraction>0.5f?base.Epsylon:_Cutoff;
    }
}