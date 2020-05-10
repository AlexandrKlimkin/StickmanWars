using System;

namespace UnityEngine.UI {
    public class UIGraphicsInterpolator : UIInterpolator {

        [Serializable]
        public class GraphicsInterpolationSettings {
            public bool InterpolateColor = true;
        }

        public GraphicsInterpolationSettings Settings;
        public Graphic GraphicTarget;
        public Color StartColor;
        public Color FinishColor;

        protected override void OnSetInterpolationFraction(float fraction) {
            if (Settings == null)
                return;
            if (Settings.InterpolateColor)
                GraphicTarget.color = Color.Lerp(StartColor, FinishColor, fraction);
        }
    }
}
