using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using Character.Control;
using Character.Movement;
using UnityEngine;

public class JoystickAim : IAimProvider
{
    public Vector2 AimPoint => CalculateAimPoint();

    private readonly Transform _HandTransform;
    private readonly MovementController _MovementController;
    private readonly string _HorAxisName;
    private readonly string _VertAxisName;

    public JoystickAim(Transform handTransform, MovementController movementController, string horAxisName, string vertAxisName)
    {
        _HandTransform = handTransform;
        _MovementController = movementController;
        _HorAxisName = horAxisName;
        _VertAxisName = vertAxisName;
    }

    private Vector2 CalculateAimPoint()
    {
        var hor = Input.GetAxis(_HorAxisName);
        var vert = Input.GetAxis(_VertAxisName);
        Vector2 vector;
        if(Mathf.Abs(hor) < 0.1f && Mathf.Abs(vert) < 0.1f)
            vector = new Vector2(_MovementController.Direction, 0) * 20f;
        else
            vector = new Vector2(hor, -vert).normalized * 20f;
        return _HandTransform.position.ToVector2() + vector;
    }
}