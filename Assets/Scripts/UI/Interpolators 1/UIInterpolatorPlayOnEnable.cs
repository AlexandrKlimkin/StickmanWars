using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI {
    public class UIInterpolatorPlayOnEnable : MonoBehaviour {
        [SerializeField]
        private float _StartValue = 0;
        [SerializeField]
        private float _TargetValue = 1;
        [SerializeField]
        private float _StartDelay;
        [SerializeField]
        private UIInterpolatorDriver _Driver;

        private void OnEnable() {
            _Driver.SetTargetFraction(_StartValue);
            _Driver.SnapToTarget();
            if (_StartDelay>0) {
                StartCoroutine(DelayedStart());
            } else {
                _Driver.SetTargetFraction(_TargetValue);
            }
        }

        private IEnumerator DelayedStart() {
            yield return new WaitForSeconds(_StartDelay);
            _Driver.SetTargetFraction(_TargetValue);
        }
    }
}