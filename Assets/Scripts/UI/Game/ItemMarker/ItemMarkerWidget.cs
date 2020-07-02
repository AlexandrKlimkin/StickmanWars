using Game.CameraTools;
using KlimLib.ResourceLoader;
using System.Collections.Generic;
using System.Linq;
using UI.Markers;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace RC.UI.Markers {
    public class ItemMarkerWidget : MarkerWidget<ItemMarkerData> {

        [SerializeField]
        private GameObject _MarkerRoot;
        [SerializeField]
        private UIInterpolator _Interpolator;
        [SerializeField]
        private Image _PreviewImage;

        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;

        private Bounds _CameraBounds;
        private bool _InCameraRect;
        private float _MinSizeCoef = 2f;
        private Vector2 _ScreenPos;
        private string _CachedId;

        protected override void Awake() {
            base.Awake();
            ContainerHolder.Container.BuildUp(this);
        }

        protected override Vector2 TransformPosition(Vector3 position) {
            _ScreenPos = base.TransformPosition(position);
            var closestPoint = GetClosestPointInRect(_ScreenPos);
            transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, closestPoint));
            return closestPoint;
        }

        private Vector2 GetClosestPointInRect(Vector2 pos) {
            _CameraBounds = new Bounds(Vector3.zero, new Vector3(((RectTransform)RectTransform.parent).rect.width, ((RectTransform)RectTransform.parent).rect.height));
            _InCameraRect = _CameraBounds.Contains(pos);
            if (_InCameraRect)
                return pos;
            var dir = pos;
            var pointOnBounds = _CameraBounds.ClosestPoint(dir);
            return pointOnBounds;
        }

        protected override void HandleData(ItemMarkerData data) {
            _MarkerRoot.SetActive(!_InCameraRect);
            if (!_InCameraRect) {
                var distFromCenter = _ScreenPos.magnitude;
                var minFractionDist = _CameraBounds.extents.x;
                var maxFractionDist = minFractionDist * _MinSizeCoef;
                var fraction = Mathf.InverseLerp(maxFractionDist, minFractionDist, distFromCenter);
                _Interpolator.SetFraction(fraction);
                if (data.WeaponId != _CachedId) {
                    var sprite = _ResourceLoader.LoadResource<Sprite>(Path.Resources.WeaponPreview(data.WeaponId));
                    _PreviewImage.sprite = sprite;
                }
                _CachedId = data.WeaponId;
                _PreviewImage.transform.rotation = Quaternion.Euler(0, 0, 15f);
            }
        }
    }
}