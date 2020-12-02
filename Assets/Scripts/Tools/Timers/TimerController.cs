using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KlimLib.Timers {
    public class TimerController : MonoBehaviour {
        [SerializeField]
        protected Text _TimeText;
        [SerializeField]
        protected bool _UseUnscaledDeltaTime;

        protected float _Timer;

        public void StartDescendingTimer(float time, Action callback) {
            StopTimer();
            StartCoroutine(DescendingTimerRoutine(time, callback));
        }

        private IEnumerator DescendingTimerRoutine(float time, Action callback) {
            _Timer = time;
            while (_Timer > 0) {
                _Timer -= _UseUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
                _TimeText.text = GetTimeText(_Timer);
                yield return null;
            }
            callback?.Invoke();
        }

        public void StartAscendingTimer() {
            StopTimer();
            StartCoroutine(AscendingTimerRoutine());
        }

        private IEnumerator AscendingTimerRoutine() {
            while (true) {
                _Timer += _UseUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
                _TimeText.text = GetTimeText(_Timer);
                yield return null;
            }
        }

        public void StopTimer() {
            StopAllCoroutines();
        }

        protected virtual string GetTimeText(float time) {
            return ((int)time).ToString();
        }
    }
}
