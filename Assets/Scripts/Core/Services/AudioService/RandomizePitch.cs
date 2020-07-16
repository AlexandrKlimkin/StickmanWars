using UnityEngine;

namespace Core.Audio {
    public class RandomizePitch : MonoBehaviour {
        [Interval(-3f, 3f)]
        public Vector2 PitchRange;

        private AudioSource _AudioSource;

        private void Awake() {
            _AudioSource = GetComponent<AudioSource>();
        }

        private void OnEnable() {
            var randPitch = UnityEngine.Random.Range(PitchRange.x, PitchRange.y);
            _AudioSource.pitch = randPitch;
        }
    }
}
