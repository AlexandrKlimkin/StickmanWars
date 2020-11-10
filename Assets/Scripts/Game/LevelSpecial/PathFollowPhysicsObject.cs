using Assets.Scripts.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial {
    public class PathFollowPhysicsObject : MonoBehaviour {
        public List<FollowPointData> FollowPoints;
        public float Speed;
        public float DestinationReachDist = 0.5f;
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
                var delta = _CurrentFollowPoint.Transform.position - transform.position;
                var distance = delta.magnitude;
                var normilizedVelocity = delta.normalized;
                var velocity = normilizedVelocity * Speed * Time.fixedDeltaTime;
                _Rigidbody.MovePosition(_Rigidbody.position + velocity.ToVector2());
                if (distance < DestinationReachDist) {
                    IsMoving = false;
                    transform.position = _CurrentFollowPoint.Transform.position;
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
            transform.position = _CurrentFollowPoint.Transform.position;
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
