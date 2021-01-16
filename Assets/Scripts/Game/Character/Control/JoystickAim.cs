using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using Character.Control;
using Character.Movement;
using InControl;
using UnityEngine;

public class JoystickAim : IAimProvider
{
    public Vector2 AimPoint => CalculateAimPoint();

    private readonly Transform _HandTransform;
    private readonly MovementController _MovementController;

    private readonly PlayerActions _PlayerActions;

    public JoystickAim(Transform handTransform, MovementController movementController, PlayerActions playerActions)
    {
        _HandTransform = handTransform;
        _MovementController = movementController;
        _PlayerActions = playerActions;
    }

    private Vector2 CalculateAimPoint()
    {
        var hor = _PlayerActions.Aim.Value.x;
        var vert = -_PlayerActions.Aim.Value.y;
        //Debug.LogError($"{_HorAxisName} - {hor},{_VertAxisName} - {vert}");
        Vector2 vector;
        if(Mathf.Abs(hor) < 0.1f && Mathf.Abs(vert) < 0.1f)
            vector = _HandTransform.transform.position + new Vector3(_MovementController.Direction * 200f, 0.7f, 0);
        else
            vector = _HandTransform.transform.position + new Vector3(hor, -vert).normalized * 200f + Vector3.up * 0.7f;
        return vector;
    }
}