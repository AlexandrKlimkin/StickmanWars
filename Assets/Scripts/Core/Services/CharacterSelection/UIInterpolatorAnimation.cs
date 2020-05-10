using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MapSelection.UI {
    public class UIInterpolatorAnimation : MonoBehaviour {
        [SerializeField]
        private UIInterpolatorGroup _Group;
        [SerializeField]
        private AnimationCurve _Curve;
        [SerializeField]
        private float _AnimationTime;

        public void Play() {
            StopAllCoroutines();
            StartCoroutine(ArrowAnimationRoutine());
        }

        private IEnumerator ArrowAnimationRoutine() {
            var timer = 0f;
            while (timer < _AnimationTime) {
                _Group.SetFraction(_Curve.Evaluate(timer / _AnimationTime));
                timer += Time.deltaTime;
                yield return null;
            }
            _Group.SetFraction(_Curve.Evaluate(1));
        }
    }
}
