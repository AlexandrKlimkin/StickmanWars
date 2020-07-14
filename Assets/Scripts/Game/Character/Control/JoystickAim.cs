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
            vector = _HandTransform.transform.position + new Vector3(_MovementController.Direction * 200f, 0.7f, 0);
        else
            vector = _HandTransform.transform.position + new Vector3(hor, -vert).normalized * 200f + Vector3.up * 0.7f;
        return vector;
    }
}