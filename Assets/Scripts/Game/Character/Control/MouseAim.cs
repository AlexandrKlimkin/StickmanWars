using System.Collections;
using System.Collections.Generic;
using Character.Control;
using UnityEngine;

public class MouseAim : IAimProvider
{
    private readonly Camera _Camera;
    public MouseAim(Camera camera)
    {
        _Camera = camera;
    }
    public Vector2 AimPoint => _Camera.ScreenToWorldPoint(Input.mousePosition);
}
