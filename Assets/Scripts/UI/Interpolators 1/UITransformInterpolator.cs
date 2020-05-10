using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

namespace UnityEngine.UI {
    [AddComponentMenu("Layout/UI Transform Interpolator", 102)]
    /// <summary>
    ///   Layout class to arrange children elements in a grid format.
    /// </summary>
    /// <remarks>
    /// The GridLayoutGroup component is used to layout child layout elements in a uniform grid where all cells have the same size. The size and the spacing between cells is controlled by the GridLayoutGroup itself. The children have no influence on their sizes.
    /// </remarks>
    public class UITransformInterpolator : UIInterpolator {

        [Serializable]
        public class TransformInterpolationSettings {
            public bool InterpolatePivot = true;
            public bool InterpolateOffsetMax = false;
            public bool InterpolateOffsetMin = false;
            public bool InterpolateAnchorsMax = false;
            public bool InterpolateAnchorsMin = false;
            public bool InterpolatePosition = false;
            public bool InterpolateLocalScale = false;
        }

        public TransformInterpolationSettings Settings;
        public RectTransform TargetOverride;
        public RectTransform StartState;
        public RectTransform FinishState;

        private RectTransform Target => TargetOverride ? TargetOverride : RectTransform;

        protected override void OnSetInterpolationFraction(float fraction) {
            if (Settings == null)
                return;
            if (StartState == null || FinishState == null)
                return;
            if (Settings.InterpolatePivot)
                Target.pivot = Vector2.Lerp(StartState.pivot, FinishState.pivot, fraction);
            if (Settings.InterpolateOffsetMin)
                Target.offsetMin = Vector2.Lerp(StartState.offsetMin, FinishState.offsetMin, fraction);
            if (Settings.InterpolateOffsetMax)
                Target.offsetMax = Vector2.Lerp(StartState.offsetMax, FinishState.offsetMax, fraction);
            if (Settings.InterpolateAnchorsMin)
                Target.anchorMin = Vector2.Lerp(StartState.anchorMin, FinishState.anchorMin, fraction);
            if (Settings.InterpolateAnchorsMax)
                Target.anchorMax = Vector2.Lerp(StartState.anchorMax, FinishState.anchorMax, fraction);
            if (Settings.InterpolatePosition)
                Target.transform.position = Vector3.Lerp(StartState.position, FinishState.position, fraction);
            if (Settings.InterpolateLocalScale)
                Target.transform.localScale = Vector3.Lerp(StartState.localScale, FinishState.localScale, fraction);
        }
    }
}
