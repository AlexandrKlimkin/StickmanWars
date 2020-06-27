using System.Collections.Generic;
using System.Linq;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Markers {
    public class ItemExtentionWidget : MonoBehaviour {

        public UIInterpolator Interpolator;

        public Transform ControlPointPivot;

        private void OnEnable() {
            ContainerHolder.Container.RegisterInstance(this);
        }

        private void OnDisable() {
            ContainerHolder.Container.Unregister(this.GetType());
        }

        public void SetVisibilityFraction(float fraction) {
            Interpolator.SetFraction(fraction);
        }
    }
}