using System.Collections;
using System.Collections.Generic;
using Character.Control;
using Character.Movement;
using UnityEngine;

public class MouseAimProvider : IAimProvider {
    private readonly Camera _Camera;
    private readonly Transform _CharTransform;

    private Vector3 _ShoulderPoint => _CharTransform.position + new Vector3(0, 13f, 0);

    public MouseAimProvider(Camera camera, Transform charTransform) {
        _Camera = camera;
        _CharTransform = charTransform;
    }

    public Vector2 AimPoint {
        get {
            var mouseWorldPos = _Camera.ScreenToWorldPoint(Input.mousePosition);
            var vector = mouseWorldPos - _ShoulderPoint;
            var dist = Vector2.Distance(mouseWorldPos, _ShoulderPoint);
            if (dist < 20f) {
                var vectorNew = vector / dist * 20f;
                return _ShoulderPoint + vectorNew;
            } else {
                return _Camera.ScreenToWorldPoint(Input.mousePosition) + _CharTransform.position;
            }
        }
    }
}