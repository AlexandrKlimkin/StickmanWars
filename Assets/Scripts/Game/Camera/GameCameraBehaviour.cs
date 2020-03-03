using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEditor;
using UnityEngine;

namespace Rendering
{
    public class GameCameraBehaviour : SingletonBehaviour<GameCameraBehaviour>
    {
        public List<Unit> Targets;
        public float PositionDamping;
        public float SizeDamping;
        public Vector2 RigthDownOffset;
        public Vector2 LeftUpOffstet;
        public float MinSize = 50f;
        public float VelocityOffsetMultiplier;
        public CameraBounds CameraBounds;

        public float Zoom => _Camera.orthographicSize;

        private Camera _Camera;
        private Rect _ResultRect;

        private void Awake()
        {
            _Camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Targets == null || Targets.Count == 0)
                return;
            ClearTargets();
            _ResultRect = TargetsRect();
            _ResultRect = RectWithOffsets(_ResultRect);
            CalculateSize();
            CalculatePosition();
        }

        private void ClearTargets()
        {
            for (var i = 0; i < Targets.Count; i++)
            {
                var target = Targets[i];
                if (target) continue;
                Targets.Remove(target);
                i--;
            }
        }

        private void CalculatePosition()
        {
            var targetpos = new Vector3(_ResultRect.center.x, _ResultRect.center.y, -10f);

            var left = CameraBounds.Rect.xMin;
            var right = CameraBounds.Rect.xMax;
            var up = CameraBounds.Rect.yMin;
            var down = CameraBounds.Rect.yMax;
            var width = _Camera.orthographicSize * _Camera.aspect;
            var x = targetpos.x;
            var y = targetpos.y;
            if (x - width < left)
                x = left + width;
            if (x + width > right)
                x = right - width;
            if (y + _Camera.orthographicSize > down)
                y = down - _Camera.orthographicSize;
            if (y - _Camera.orthographicSize < up)
                y = up + _Camera.orthographicSize;

            targetpos = new Vector3(x, y, -10f);
            transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * PositionDamping);
        }

        private void CalculateSize()
        {
            var aspect = _Camera.aspect;
            var width = _ResultRect.width;
            var height = _ResultRect.height;
            var targetSize = width / height > aspect ? width / _Camera.aspect : height;
            targetSize *= 0.5f;
            var maxSizeHor = CameraBounds.Rect.size.x / (2f * _Camera.aspect);
            var maxSizeVert = CameraBounds.Rect.size.y / 2f;
            var maxSize = Mathf.Min(maxSizeHor, maxSizeVert);

            targetSize = Mathf.Clamp(targetSize, MinSize, maxSize);
            _Camera.orthographicSize = Mathf.Lerp(_Camera.orthographicSize, targetSize, Time.deltaTime * SizeDamping);
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

        private void OnDrawGizmos()
        {
            MyGizmos.DrawRect(_ResultRect);
            Gizmos.color = Color.gray;
        }
    }
}