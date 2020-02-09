using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameCameraBehaviour : MonoBehaviour
{
    public List<Unit> Targets;
    public float PositionDamping;
    public float SizeDamping;
    public Vector2 RigthDownOffset;
    public Vector2 LeftUpOffstet;
    public float MinSize = 50f;
    public float VelocityOffsetMultiplier;

    private Camera _Camera;
    private Rect _TargetsRect;
    private Rect _OffsetsRect;

    private void Awake()
    {
        _Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Targets == null || Targets.Count == 0)
            return;
        _TargetsRect = TargetsRect();
        _OffsetsRect = RectWithOffsets(_TargetsRect);
        CalculatePosition();
        CalculateSize();
    }

    private void CalculatePosition()
    {
        var targetpos = new Vector3(_OffsetsRect.center.x, _OffsetsRect.center.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * PositionDamping);
    }

    private void CalculateSize()
    {
        var aspect = _Camera.aspect;
        var width = _OffsetsRect.width;
        var height = _OffsetsRect.height;
        var targetSize = width / height > aspect ? width / _Camera.aspect : height;
        targetSize *= 0.5f;
        if (targetSize < MinSize)
            targetSize = MinSize;
        _Camera.orthographicSize = Mathf.Lerp(_Camera.orthographicSize, targetSize, Time.deltaTime * SizeDamping);
    }

    private Rect TargetsRect()
    {
        var minY = Targets.Min(_ => _.Position.y + Mathf.Clamp(_.Velocity.y, float.MinValue, 0) * VelocityOffsetMultiplier);
        var maxY = Targets.Max(_ => _.Position.y + Mathf.Clamp(_.Velocity.y, 0, float.MaxValue) * VelocityOffsetMultiplier);
        var minX = Targets.Min(_ => _.Position.x + Mathf.Clamp(_.Velocity.x, float.MinValue, 0) * VelocityOffsetMultiplier);
        var maxX = Targets.Max(_ => _.Position.x + Mathf.Clamp(_.Velocity.x, 0, float.MaxValue) * VelocityOffsetMultiplier);
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
        MyGizmos.DrawRect(_TargetsRect);
        Gizmos.color = Color.gray;
        MyGizmos.DrawRect(_OffsetsRect);
    }
}