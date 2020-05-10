using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace UnityEngine.UI {
    [RequireComponent(typeof(UIInterpolatorGroup))]
    public abstract class UIInterpolatorDriver : UIBehaviour {

        [Range(0, 1)]
        [SerializeField]
        private float _TargetFraction;
        public bool UnscaledDeltaTime = true;
        
        protected float CurrentFraction { get; private set; }
        protected UIInterpolatorGroup Interpolator { get; private set; }

        public float TargetFraction => _TargetFraction;
        protected virtual float Epsylon => 0.001f;

        protected float _AnimationTimeScale { get; private set; }
        private Coroutine _CurrentUpdateFractionTask;

        protected float DeltaTime {
            get {
                return UnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
            }
        }

        private bool _Initialized;

        private void Initialize() {
            if (_Initialized)
                return;
            _Initialized = true;
            OnInitialize();
        }

        protected override void Awake() {
            SnapToTarget();
        }

        private void WakeUp() {
            if (gameObject.activeInHierarchy && _CurrentUpdateFractionTask == null)
                _CurrentUpdateFractionTask = StartCoroutine(UpdateFractionTask());
        }

        protected override void OnDisable() {
            base.OnDisable();
            _CurrentUpdateFractionTask = null;
        }

        protected virtual IEnumerator UpdateFractionTask() {
            while (true) {
                yield return null;
                if (CheckTransitionComplete()) {
                    SnapToTarget();
                    break;
                }
                
                var newFraction = GetNextFraction();
                CurrentFraction = newFraction;
                ApplyFraction();
            }
            _CurrentUpdateFractionTask = null;
        }

        protected virtual bool CheckTransitionComplete() {
            return Mathf.Abs(CurrentFraction - _TargetFraction) < Epsylon;
        }

        protected virtual void OnInitialize() {
            Interpolator = this.GetComponent<UIInterpolatorGroup>();
        }

        protected virtual void ApplyFraction() {
            Initialize();
            Interpolator.SetFraction(CurrentFraction);
        }

        protected abstract float GetNextFraction();

        private Action _Callback;

        public void SetTargetFraction(float targetFraction) {
            SetTargetFraction(targetFraction, null, true);
        }

        public void SetTargetFraction(float targetFraction, Action callback = null, bool resetScale = true) {
            _Callback = callback;
            if (resetScale)
                _AnimationTimeScale = 1;
            _TargetFraction = targetFraction;
            WakeUp();
        }
        public void SetAnimationSpeed(float speed) {
            _AnimationTimeScale = speed;
        }

        protected virtual void ProcessSnap() { }

        public void SnapToTarget() {
            CurrentFraction = _TargetFraction;
            ProcessSnap();
            _CurrentUpdateFractionTask = null;
            ApplyFraction();
            if (_Callback != null) {
                var callbackClone = _Callback;
                _Callback = null;
                callbackClone.Invoke();
            }
        }
    }
}
