using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

namespace UnityEngine.UI {
    [AddComponentMenu("Layout/UI Layout Element Interpolator", 102)]
    /// <summary>
    ///   Layout class to arrange children elements in a grid format.
    /// </summary>
    /// <remarks>
    /// The GridLayoutGroup component is used to layout child layout elements in a uniform grid where all cells have the same size. The size and the spacing between cells is controlled by the GridLayoutGroup itself. The children have no influence on their sizes.
    /// </remarks>
    public class UILayoutElementInterpolator : UIInterpolator {

        [Serializable]
        public class TransformInterpolationSettings {
            public bool InterpolatePreferredHeight = true;
            public bool InterpolatePreferredWidth = false;
        }

        [Serializable]
        public class LayoutElementState {
            public float PreferredHeight = 0;
            public float PreferredWidth = 0;
        }

        public TransformInterpolationSettings Settings;
        public LayoutElement TargetOverride;
        public LayoutElementState StartState;
        public LayoutElementState FinishState;

        private LayoutElement _Target;

        protected override void OnInitialize() {
            _Target = TargetOverride ? TargetOverride : this.GetComponent<LayoutElement>();
        }

        protected override void OnSetInterpolationFraction(float fraction) {
            if (Settings == null)
                return;
            if (StartState == null || FinishState == null)
                return;
            if (Settings.InterpolatePreferredHeight)
                _Target.preferredHeight = Mathf.Lerp(StartState.PreferredHeight, FinishState.PreferredHeight, fraction);
            if (Settings.InterpolatePreferredWidth)
                _Target.preferredWidth = Mathf.Lerp(StartState.PreferredWidth, FinishState.PreferredWidth, fraction);
        }
    }
}
