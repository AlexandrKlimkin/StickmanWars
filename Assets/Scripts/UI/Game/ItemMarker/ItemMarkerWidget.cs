using Game.CameraTools;
using System.Collections.Generic;
using System.Linq;
using UI.Markers;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace RC.UI.Markers {
    public class ItemMarkerWidget : MarkerWidget<ItemMarkerData> {
        private const float MarkerDampening = 7;

        [SerializeField]
        private PivotedArrowWidget _Pivot;

        [SerializeField]
        private float _InnerRadius;
        [SerializeField]
        private float _OuterRadius;
        [SerializeField]
        private float _TopPadding = 100;

        public UIInterpolator Interpolator;
        public AnimationCurve PositionInterpolationCurve;
        public AnimationCurve ExtentionInterpolationCurve;

        [Dependency]
        private readonly MainCamera _MainCamera;

        private static BorderDeclampHandler _Left, _Right;
        private bool _IAmBoss;
        private int _LeftID;
        private int _RightID;
        private float _HighlightFraction;
        private float _LastFraction;
        private ItemExtentionWidget _ItemExtentionWidget;
        private Canvas _Canvas;


        protected override void Start() {
            base.Start();
            if (_Left == null) {
                _IAmBoss = true;
                _Left = new BorderDeclampHandler();
                _Right = new BorderDeclampHandler();
            }
            _LeftID = _Left.Register(0, 0);
            _RightID = _Right.Register(0, 0);
            //_ControlPointExtentionWidget = ContainerHolder.Container.Resolve<ControlPointExtentionWidget>();
            _Canvas = this.transform.root.GetComponent<Canvas>();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if (_IAmBoss) {
                _Left = null;
                _Right = null;
                _IAmBoss = false;
            }
        }

        protected override void HandleData(ItemMarkerData data) {
            _HighlightFraction = data.HighlightFraction;
        }

        protected override Vector2 TransformPosition(Vector3 position) {
            var radialOffset = GetEulerAsimuth(_MainCamera.Camera.transform, position);
            var FOVlimits = GetEulerAsimuth(_MainCamera.Camera.transform, _MainCamera.Camera.ViewportToWorldPoint(new Vector3(1, 1.5f, _MainCamera.Camera.nearClipPlane)));

            if (_IAmBoss) {
                _Left.Process();
                _Right.Process();
            }

            var asimuth = radialOffset.y;

            radialOffset.x = Mathf.Clamp(radialOffset.x, FOVlimits.x, 0);// * OffsightWeight(Mathf.Abs(radialOffset.y), FOVlimits.y);
            radialOffset.y = Mathf.Clamp(radialOffset.y, -FOVlimits.y, FOVlimits.y);

            var screenpos = base.TransformPosition(_MainCamera.Camera.transform.TransformPoint(Quaternion.Euler(radialOffset) * Vector3.forward * 5));


            var halfSize = ((RectTransform)transform.parent).rect.size / 2;
            var leftWeight = 1 - AndleWeight(halfSize.x + screenpos.x, _InnerRadius * 5, _InnerRadius * 5);
            var rightWeight = 1 - AndleWeight(halfSize.x - screenpos.x, _InnerRadius * 5, _InnerRadius * 5);

            var leftPosition = _Left.GetAdjustedPosition(_LeftID, screenpos.y, leftWeight * (_InnerRadius * 2.4f));
            var rightPosition = _Right.GetAdjustedPosition(_RightID, screenpos.y, rightWeight * (_InnerRadius * 2.4f));


            var edgeOffset = halfSize.x - Mathf.Abs(screenpos.x);
            if (edgeOffset <= _OuterRadius) {
                _Pivot.SetRotation(-90 * Mathf.Sign(screenpos.x));
                screenpos.x = (halfSize.x - _OuterRadius) * Mathf.Sign(screenpos.x);
            } else {
                _Pivot.SetRotation(180);
            }
            screenpos.y = Mathf.Clamp(Mathf.Lerp(Mathf.Lerp(screenpos.y, leftPosition, leftWeight), rightPosition, rightWeight), _OuterRadius - halfSize.y, halfSize.y - _TopPadding - _OuterRadius);

            var result = screenpos;
            if (_HighlightFraction > 0 || _LastFraction > 0) {
                result = RefreshHighlightAnimation(screenpos);
                _LastFraction = _HighlightFraction;
            }

            return result;
        }

        private Vector3 RefreshHighlightAnimation(Vector3 widgetScreenPos) {
            //if (_ControlPointExtentionWidget == null)
            //    return widgetScreenPos;

            //_ControlPointExtentionWidget.SetVisibilityFraction(ExtentionInterpolationCurve.Evaluate(_HighlightFraction));
            Interpolator.SetFraction(PositionInterpolationCurve.Evaluate(_HighlightFraction));
            var pivotPos = _Canvas.transform.InverseTransformPoint(_ItemExtentionWidget.ControlPointPivot.position);
            var resultPos = Vector3.Lerp(widgetScreenPos, pivotPos, PositionInterpolationCurve.Evaluate(_HighlightFraction));
            return resultPos;
        }

        private static float OffsightWeight(float actualAngle, float cutoffAngle) {
            return 1 - Mathf.Clamp01((actualAngle - cutoffAngle) / (180 - cutoffAngle));
        }

        private static Vector3 GetEulerAsimuth(Transform observer, Vector3 position) {
            var result = Quaternion.LookRotation(observer.InverseTransformPoint(position)).eulerAngles;
            if (result.y > 180)
                result.y -= 360;
            if (result.x > 180)
                result.x -= 360;
            return result;
        }

        private static float AndleWeight(float edgeOffset, float offset, float width) {
            return Mathf.Clamp01((edgeOffset - offset) / width);
        }

        private class BorderDeclampHandler {
            private class ConrolPointTracker {
                public float DesiredPosition;
                private float _AdjustedPosition;
                public float AdjustedPosition {
                    get {
                        return _AdjustedPosition;
                    }
                    set {
                        _AdjustedPosition = Mathf.Lerp(_AdjustedPosition, value, Time.deltaTime * MarkerDampening);
                    }
                }
                public float DesiredWidth;

                public float Bottom => DesiredPosition - DesiredWidth / 2;

                public float Top => DesiredPosition + DesiredWidth / 2;

            }

            private List<ConrolPointTracker> _Trackers = new List<ConrolPointTracker>();

            public int Register(float requiredPosition, float requiredWidth) {
                _Trackers.Add(new ConrolPointTracker() { DesiredPosition = requiredPosition, AdjustedPosition = requiredPosition, DesiredWidth = requiredWidth });
                return _Trackers.Count - 1;
            }

            public void Process() {
                _Trackers.Where(_ => _.DesiredWidth <= 0.01f).ForEach(_ => _.AdjustedPosition = _.DesiredPosition);
                using (var sorted = _Trackers.Where(_ => _.DesiredWidth > 0.01f).OrderBy(_ => _.DesiredPosition).GetEnumerator()) {
                    var curBatch = new List<ConrolPointTracker>();
                    var minPos = 0f;
                    var elementRead = false;
                    while (elementRead || sorted.MoveNext()) {
                        var item = sorted.Current;
                        var batchBottom = Mathf.Max(item.Bottom, minPos);
                        var batchHeight = item.DesiredWidth;
                        curBatch.Add(sorted.Current);
                        while ((elementRead = sorted.MoveNext()) && (batchBottom + batchHeight) >= sorted.Current.Bottom) {
                            batchHeight += sorted.Current.DesiredWidth;
                            curBatch.Add(sorted.Current);
                        }
                        curBatch.ForEach(_ => {
                            _.AdjustedPosition = batchBottom + _.DesiredWidth / 2;
                            batchBottom += _.DesiredWidth;
                        });
                        curBatch.Clear();
                        minPos = batchBottom;
                    }
                }
            }

            public float GetAdjustedPosition(int index, float requiredPosition, float requiredWidth) {
                var item = _Trackers[index];
                item.DesiredPosition = requiredPosition;
                item.DesiredWidth = requiredWidth;

                return item.AdjustedPosition;
            }
        }
    }
}