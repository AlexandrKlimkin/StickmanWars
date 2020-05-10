using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI {
    public class InterpolatorBlinkManager : MonoBehaviour {
        [SerializeField]
        private UIInterpolatorDriver _Driver;

        public void Blink(Action onHidden, Action onComplete) {
            _Driver.SetTargetFraction(0, () => {
                onHidden?.Invoke();
                _Driver.SetTargetFraction(1, onComplete);
            });
        }
    }
}