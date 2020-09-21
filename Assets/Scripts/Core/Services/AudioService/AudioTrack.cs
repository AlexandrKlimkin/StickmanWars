using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio {
    public class AudioTrack : AudioEffect {
        private AudioSource _AudioSource;
        private float _OriginalVolume;
        private float _OriginalMaxDistance;

        private float _CurrnetVolume => _OriginalVolume;

        public override bool IsPlaying => _AudioSource.isPlaying;

        protected override void Awake() {
            base.Awake();
            _AudioSource = GetComponent<AudioSource>();
            _OriginalVolume = _AudioSource.volume;
            _OriginalMaxDistance = _AudioSource.maxDistance;
        }

        protected override void OnEnable() {
            base.OnEnable();
            _AudioSource.volume = _CurrnetVolume;
        }

        protected override void ResetVolume() {
            _AudioSource.volume = _CurrnetVolume;
        }

        protected override void PlayAudio() {
            _AudioSource.Play();
        }

        public override void StopAudio() {
            _AudioSource.Stop();
        }

        protected override IEnumerator FadeAndStopTask() {
            while (_AudioSource.volume > 0) {
                _AudioSource.volume -= _CurrnetVolume * Time.unscaledDeltaTime / FadeTime;
                yield return null;
            }
            _AudioSource.volume = 0;
            StopAudio();
            gameObject.SetActive(false); 
        }

        protected override IEnumerator PlayAndRiseTask() {
            _AudioSource.volume = 0;
            _AudioSource.Play();
            while (_AudioSource.volume <= _CurrnetVolume) {
                _AudioSource.volume += _CurrnetVolume * Time.unscaledDeltaTime / FadeTime;
                yield return null;
            }
            _AudioSource.volume = _CurrnetVolume;
        }
    }
}