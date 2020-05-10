using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

namespace UnityEngine.UI {
    [AddComponentMenu("Layout/UI Canvas Group Interpolator", 102)]
    /// <summary>
    ///   Layout class to arrange children elements in a grid format.
    /// </summary>
    /// <remarks>
    /// The GridLayoutGroup component is used to layout child layout elements in a uniform grid where all cells have the same size. The size and the spacing between cells is controlled by the GridLayoutGroup itself. The children have no influence on their sizes.
    /// </remarks>
    public class UICanvasGroupInterpolator : UIInterpolator {

        public CanvasGroup TargetOverride;
        public float StartAlpha = 0;
        public float FinishAlpha = 1;
        public bool DisableOnZero = true;
        public bool ModifyBlockRaycasts = false;
        public float BlockRaycastThreshold = 0.5f;        

        private CanvasGroup _CanvasGroup;

        protected override void OnInitialize() {
            _CanvasGroup = TargetOverride ? TargetOverride : this.GetComponent<CanvasGroup>();
        }

        protected override void OnSetInterpolationFraction(float fraction) {
            if (_CanvasGroup == null)
                return;
            _CanvasGroup.alpha = Mathf.Lerp(StartAlpha, FinishAlpha, fraction);
            if (DisableOnZero)
                _CanvasGroup.gameObject.SetActive(_CanvasGroup.alpha > 0);
            if (ModifyBlockRaycasts)
                _CanvasGroup.blocksRaycasts = fraction > BlockRaycastThreshold;
        }
    }
}
