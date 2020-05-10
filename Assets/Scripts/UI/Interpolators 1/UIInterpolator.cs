using UnityEngine.EventSystems;

namespace UnityEngine.UI {

    public abstract class UIInterpolator : UIBehaviour {

        [Range(0, 1)]
        [SerializeField]
        protected float _Fraction;

        [SerializeField]
        protected bool _Inverted;

        [HideInInspector]
        [SerializeField]
        protected bool _Remap;

        [HideInInspector]
        [SerializeField]
        private AnimationCurve _RemapCurve;

        protected bool Initialized { get; private set; }
        protected RectTransform RectTransform { get; private set; }


#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            Initialize(true);
            UnityEditor.EditorApplication.update += RefreshFractionEditor;
        }

        private void RefreshFractionEditor() {
            UnityEditor.EditorApplication.update -= RefreshFractionEditor;
            SetInterpolationFraction(_Fraction);
        }
#endif

        protected void Initialize(bool force = false) {
            if (!force && Initialized)
                return;
            Initialized = true;
            RectTransform = this.transform as RectTransform;
            OnInitialize();
        }

        protected virtual void OnInitialize() {

        }

        public float GetFraction() {
            return _Fraction;
        }

        public void SetFraction(float fraction) {
            _Fraction = fraction;
            SetInterpolationFraction(_Remap ? _RemapCurve.Evaluate(_Fraction) : _Fraction);
        }

        private void SetInterpolationFraction(float fraction) {
            Initialize();
            OnSetInterpolationFraction(_Inverted ? 1 - fraction : fraction);
        }

        protected abstract void OnSetInterpolationFraction(float fraction);
    }
}
