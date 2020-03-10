using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class PathFollowPhysicsObject : MonoBehaviour
    {
        public List<FollowPointData> FollowPoints;
        public float Speed;

        private Rigidbody2D _Rigidbody;
        public int CurrentFollowPointIndex { get; private set; }
        public bool IsMoving { get; private set; }
        private FollowPointData _CurrentFollowPoint;

        private void Awake()
        {
            _Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            CurrentFollowPointIndex = 0;
            _CurrentFollowPoint = FollowPoints[CurrentFollowPointIndex];
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
            IsMoving = true;
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
            IsMoving = false;
            yield return new WaitForSeconds(_CurrentFollowPoint.WaitTime);
        }

        private void SwitchFollowPoint()
        {
            var totalPoints = FollowPoints.Count;
            CurrentFollowPointIndex = (CurrentFollowPointIndex + 1) % totalPoints;
            _CurrentFollowPoint = FollowPoints[CurrentFollowPointIndex];
        }
    }

    [Serializable]
    public class FollowPointData
    {
        public Transform Transform;
        public float WaitTime;
    }
}
