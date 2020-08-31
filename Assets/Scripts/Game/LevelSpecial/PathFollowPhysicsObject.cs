using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial {
    public class PathFollowPhysicsObject : MonoBehaviour {
        public List<FollowPointData> FollowPoints;
        public float Speed;

        private Rigidbody2D _Rigidbody;
        public int CurrentFollowPointIndex { get; private set; }
        public bool IsMoving { get; private set; }
        private FollowPointData _CurrentFollowPoint;

        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
            CurrentFollowPointIndex = 0;
            _CurrentFollowPoint = FollowPoints[CurrentFollowPointIndex];
        }

        private void FixedUpdate() {
            if (IsMoving) {
                var delta = (Vector2)(_CurrentFollowPoint.Transform.position - transform.position);
                var distance = delta.magnitude;
                var normilizedVelocity = delta.normalized;
                var moveVector = normilizedVelocity * Speed;
                moveVector = Vector2.ClampMagnitude(moveVector, distance);
                _Rigidbody.position += moveVector;
                if (_TargetPointReached) {
                    IsMoving = false;
                }
            }
        }

        public void MoveToNextPoint() {
            SwitchFollowPoint();
            IsMoving = true;
        }

        public void ResetToPoint(int index) {
            if (index >= FollowPoints.Count)
                return;
            IsMoving = false;
            CurrentFollowPointIndex = index;
            _CurrentFollowPoint = FollowPoints[CurrentFollowPointIndex];
            _Rigidbody.position = _CurrentFollowPoint.Transform.position;
            _Rigidbody.velocity = Vector2.zero;
        }

        private bool _TargetPointReached {
            get {
                var distance = Vector2.Distance(_CurrentFollowPoint.Transform.position, transform.position);
                return distance < 0.1f;
            }
        }

        private void SwitchFollowPoint() {
            var totalPoints = FollowPoints.Count;
            CurrentFollowPointIndex = (CurrentFollowPointIndex + 1) % totalPoints;
            _CurrentFollowPoint = FollowPoints[CurrentFollowPointIndex];
        }
    }

    [Serializable]
    public class FollowPointData {
        public Transform Transform;
    }
}
