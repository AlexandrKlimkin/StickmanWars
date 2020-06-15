using System.Collections;
using UnityEngine;

namespace Game.Physics {
    public class EffectorExplosion : MonoBehaviour {
        [Button("Play")]
        public bool PlayButton;
        public bool PlayOnStart;
        public bool PlayOnEnable;

        private PointEffector2D _PointEffector;
        private bool _Effected = true;

        private void OnEnable() {
            if (PlayOnEnable)
                Play();
        }

        private void Awake() {
            _PointEffector = GetComponent<PointEffector2D>();
            _PointEffector.enabled = false;
        }

        private void Start() {
            if (PlayOnStart)
                Play();
        }

        private void FixedUpdate() {
            _PointEffector.enabled = !_Effected;
            if (!_Effected) {
                _Effected = true;
            }
        }

        //private IEnumerator PlayRoutine() {
        //    _PointEffector.enabled = true;
        //    yield return new WaitForFixedUpdate();
        //    _PointEffector.enabled = false;
        //}

        public void Play() {
            //StopAllCoroutines();
            //StartCoroutine(PlayRoutine());
            _Effected = false;
        }

    }
}
