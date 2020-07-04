using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KlimLib.SignalBus;
using Tools;
using UnityDI;
using UnityEditor;
using UnityEngine;

namespace Game.CameraTools
{
    public class GameCameraBehaviour : MonoBehaviour {
        [Dependency]
        private readonly SignalBus _SignalBus;

        public IReadOnlyList<ICameraTarget> Targets => _Targets;
        private List<ICameraTarget> _Targets;
        public float PositionDamping;
        public float SizeDamping;
        public Vector2 RigthDownOffset;
        public Vector2 LeftUpOffstet;
        public float MinSize = 50f;
        public float VelocityOffsetMultiplier;
        private CameraBounds _CameraBounds;

        public float Zoom => Camera.orthographicSize;

        public Camera Camera { get; private set; }

        private Rect _ResultRect;

        protected void Awake()
        {
            Camera = GetComponent<Camera>();
            _Targets = new List<ICameraTarget>();
        }

        private void Update()
        {
            if (Targets == null || Targets.Count == 0)
                return;
            _ResultRect = TargetsRect();
            _ResultRect = RectWithOffsets(_ResultRect);
            CalculateSize();
            CalculatePosition();
        }

        public void Initialize() {
            _SignalBus.Subscribe<GameCameraTargetsChangeSignal>(OnCameraTargetsChange, this);
        }

        public void SetBounds(CameraBounds bounds) {
            _CameraBounds = bounds;
        }

        private void CalculatePosition()
        {
            var targetpos = new Vector3(_ResultRect.center.x, _ResultRect.center.y, -100f);

            var left = _CameraBounds.Rect.xMin;
            var right = _CameraBounds.Rect.xMax;
            var up = _CameraBounds.Rect.yMin;
            var down = _CameraBounds.Rect.yMax;
            var width = Camera.orthographicSize * Camera.aspect;
            var x = targetpos.x;
            var y = targetpos.y;
            if (x - width < left)
                x = left + width;
            if (x + width > right)
                x = right - width;
            if (y + Camera.orthographicSize > down)
                y = down - Camera.orthographicSize;
            if (y - Camera.orthographicSize < up)
                y = up + Camera.orthographicSize;

            targetpos = new Vector3(x, y, -100f);
            transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * PositionDamping);
        }

        private void CalculateSize()
        {
            var aspect = Camera.aspect;
            var width = _ResultRect.width;
            var height = _ResultRect.height;
            var targetSize = width / height > aspect ? width / Camera.aspect : height;
            targetSize *= 0.5f;
            var maxSizeHor = _CameraBounds.Rect.size.x / (2f * Camera.aspect);
            var maxSizeVert = _CameraBounds.Rect.size.y / 2f;
            var maxSize = Mathf.Min(maxSizeHor, maxSizeVert);

            targetSize = Mathf.Clamp(targetSize, MinSize, maxSize);
            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, targetSize, Time.deltaTime * SizeDamping);
        }

        private Rect TargetsRect()
        {
            var minY = Targets.Min(_ =>
                _.Position.y + Mathf.Clamp(_.Velocity.y, float.MinValue, 0) * VelocityOffsetMultiplier);
            var maxY = Targets.Max(_ =>
                _.Position.y + Mathf.Clamp(_.Velocity.y, 0, float.MaxValue) * VelocityOffsetMultiplier);
            var minX = Targets.Min(_ =>
                _.Position.x + Mathf.Clamp(_.Velocity.x, float.MinValue, 0) * VelocityOffsetMultiplier);
            var maxX = Targets.Max(_ =>
                _.Position.x + Mathf.Clamp(_.Velocity.x, 0, float.MaxValue) * VelocityOffsetMultiplier);
            var sizeX = maxX - minX;
            var sizeY = maxY - minY;
            var pos = new Vector2(minX, minY);
            var size = new Vector2(sizeX, sizeY);
            return new Rect(pos, size);
        }

        private Rect RectWithOffsets(Rect rect)
        {
            var pos = rect.position + LeftUpOffstet;
            var size = rect.size + RigthDownOffset - LeftUpOffstet;
            var newRect = new Rect(pos, size);
            return newRect;
        }

        private void OnCameraTargetsChange(GameCameraTargetsChangeSignal signal) {
            foreach (var pair in signal.Pairs) {
                if(pair.Item2)
                    _Targets.Add(pair.Item1);
                else {
                    if (_Targets.Contains(pair.Item1))
                        _Targets.Remove(pair.Item1);
                }
            }
        }

        private void OnDrawGizmos()
        {
            MyGizmos.DrawRect(_ResultRect);
            Gizmos.color = Color.gray;
        }

        private void OnDestroy() {
            _SignalBus?.UnSubscribeFromAll(this);
            ContainerHolder.Container.Unregister<GameCameraBehaviour>();
        }
    }
}