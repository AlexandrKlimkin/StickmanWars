using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollsDragger : MonoBehaviour
{

    Collider2D _DraggedCollider;
    Vector2 _DragOffset;

    void FixedUpdate()
    {
        var mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1)) {
            _DraggedCollider = Physics2D.OverlapPoint(mousePos);
            if (_DraggedCollider != null && _DraggedCollider.attachedRigidbody) {
                var rb = _DraggedCollider.attachedRigidbody;
                _DragOffset = mousePos - rb.position;
            }
            else {
                _DragOffset = Vector2.zero;
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            _DraggedCollider = null;
            _DragOffset = Vector2.zero;
        }
        if (_DraggedCollider) {
            var rb = _DraggedCollider.attachedRigidbody;
            rb.position = mousePos + _DragOffset;
        }

    }
}