using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowPhysicsObject : MonoBehaviour
{
    public List<FollowPointData> FollowPoints;
    public float Speed;

    private Rigidbody2D _Rigidbody;
    private int _CurrentFollowPointIndex;
    private FollowPointData _CurrentFollowPoint;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _CurrentFollowPointIndex = 0;
        _CurrentFollowPoint = FollowPoints[_CurrentFollowPointIndex];
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return MoveToPointRoutine();
            yield return WaitRoutine();
            SwitchFollowPoint();
        }
    }

    private bool _TargetPointReached
    {
        get
        {
            var distance = Vector2.Distance(_CurrentFollowPoint.Transform.position, transform.position);
            return distance < 0.1f;
        }
    }

    private IEnumerator MoveToPointRoutine()
    {
        while (!_TargetPointReached)
        {
            var delta = (Vector2) (_CurrentFollowPoint.Transform.position - transform.position);
            var distance = delta.magnitude;
            var normilizedVelocity = delta.normalized;
            var moveVector = normilizedVelocity * Speed;
            moveVector = Vector2.ClampMagnitude(moveVector, distance);
            _Rigidbody.position = _Rigidbody.position + moveVector;
            yield return null;
        }
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(_CurrentFollowPoint.WaitTime);
    }

    private void SwitchFollowPoint()
    {
        var totalPoints = FollowPoints.Count;
        _CurrentFollowPointIndex = (_CurrentFollowPointIndex + 1) % totalPoints;
        _CurrentFollowPoint = FollowPoints[_CurrentFollowPointIndex];
    }
}

[Serializable]
public class FollowPointData
{
    public Transform Transform;
    public float WaitTime;
}
