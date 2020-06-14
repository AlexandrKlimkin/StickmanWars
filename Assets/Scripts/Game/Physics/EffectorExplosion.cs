using System.Collections;
using UnityEngine;

namespace Game.Physics {
    public class EffectorExplosion : MonoBehaviour {
        [Button("Play")]
        public bool PlayButton;
        public bool PlayOnStart;

        private PointEffector2D _PointEffector;

        private void Awake() {
            _PointEffector = GetComponent<PointEffector2D>();
            _PointEffector.enabled = false;
        }

        private void Start() {
            if (PlayOnStart)
                Play();
        }

        private IEnumerator PlayRoutine() {
            _PointEffector.enabled = true;
            yield return new WaitForFixedUpdate();
            _PointEffector.enabled = false;
        }

        public void Play() {
            StopAllCoroutines();
            StartCoroutine(PlayRoutine());
        }

    }
}
