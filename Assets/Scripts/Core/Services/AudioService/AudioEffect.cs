using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityDI;
using UnityEngine.SceneManagement;
using KlimLib.SignalBus;

namespace Core.Audio {
    public abstract class AudioEffect : MonoBehaviour {
        [Dependency]
        protected readonly AudioService _AudioService;
        [Dependency]
        protected readonly SignalBus _SignalBus;

        public virtual float FadeTime => 1.0f;

        public event Action<AudioEffect> OnFinishedPlaying;
        public AudioGroup AudioGroup = AudioGroup.SFX;
        public string Name { get; private set; }

        private bool _Stopped;

        protected static int _ActiveTracks = 0;
        protected static int _MaxActiveTracks = 0;

        public abstract bool IsPlaying { get; }

        protected virtual void Awake() {
            ContainerHolder.Container.BuildUp(GetType(), this);
            //Name = Utils.RemoveClonePostfix(name);
        }

        protected virtual void OnEnable() {
            _Stopped = false;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        protected virtual void OnDisable() {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        protected virtual void OnActiveSceneChanged(Scene oldScene, Scene newScene) {
            Stop(true);
        }

        protected virtual void OnGameProgressChangedEvent(bool gameInProgress) {
            if (!gameInProgress)
                Stop(true);
        }

        public virtual void Play(bool rise) {
            StopAllCoroutines();
            if (rise) {
                StartCoroutine(PlayAndRiseTask());
            } else {
                ResetVolume();
                PlayAudio();
            }
            StartCoroutine(WaitStopWhenFinish());
        }

        protected abstract void ResetVolume();
        protected abstract void PlayAudio();
        public abstract void StopAudio();

        public virtual void Stop(bool fade) {
            if (_Stopped)
                return;
            _Stopped = true;
            OnFinishedPlaying?.Invoke(this);
            StopAllCoroutines();
            if (fade)
                StartCoroutine(FadeAndStopTask());
            else {
                StopAudio();
                gameObject.SetActive(false);
            }
        }

        private IEnumerator WaitStopWhenFinish() {
            yield return new WaitWhile(() => IsPlaying);
            Stop(false);
        }

        protected abstract IEnumerator FadeAndStopTask();

        protected abstract IEnumerator PlayAndRiseTask();

        protected void OnDestroy() {
            _AudioService.AudioPool.RemoveFromPool(this);
        }
    }
}
