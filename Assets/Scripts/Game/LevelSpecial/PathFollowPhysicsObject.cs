using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial {
    public class PathFollowPhysicsObject : MonoBehaviour {
        public List<FollowPointData> FollowPoints;
        public float Speed;

        private Rigidbody2D _Rigidbody;

        private int _CurrentFollowPointIndex;
        private FollowPointData _CurrentFollowPoint;
        public int CurrentFollowPointIndex {
            get {
                return _CurrentFollowPointIndex;
            }
            private set {
                _CurrentFollowPointIndex = value;
                _CurrentFollowPoint = FollowPoints[CurrentFollowPointIndex];
            }
        }
        public bool IsMoving { get; private set; }

        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
            CurrentFollowPointIndex = 0;
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
                    _Rigidbody.position = _CurrentFollowPoint.Transform.position;
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
            _Rigidbody.position = _CurrentFollowPoint.Transform.position;
            _Rigidbody.velocity = Vector2.zero;
        }

        private bool _TargetPointReached {
            get {
                var distance = Vector2.Distance(_CurrentFollowPoint.Transform.position, transform.position);
                //Debug.LogError($"distance = {distance}");
                return distance < 0.1f;
            }
        }

        private void SwitchFollowPoint() {
            var totalPoints = FollowPoints.Count;
            CurrentFollowPointIndex = (CurrentFollowPointIndex + 1) % totalPoints;
            //Debug.LogError($"Current Follow Point Index = {CurrentFollowPointIndex}");
        }
    }

    [Serializable]
    public class FollowPointData {
        public Transform Transform;
    }
}
