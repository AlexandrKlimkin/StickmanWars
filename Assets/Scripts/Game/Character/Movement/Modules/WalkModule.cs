using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Movement.Modules {
    public class WalkModule : MovementModule {
        public float Direction => _WalkData.Direction;

        private GroundedData _GroundedData;
        private WallSlideData _WallSlideData;
        private WalkData _WalkData;

        private WalkParameters _Parameters;
        private float _TargetXVelocity = 0f;
        public float Horizontal => _WalkData.Horizontal;
        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();

        public WalkModule(WalkParameters parameters) : base() {
            this._Parameters = parameters;
        }

        public override void Start() {
            _GroundedData = BB.Get<GroundedData>();
            _WallSlideData = BB.Get<WallSlideData>();
            _WalkData = BB.Get<WalkData>();
            CommonData.ObjTransform.GetComponentsInChildren(_SimpleCcds);
            _WalkData.Direction = 1;
        }

        public override void FixedUpdate() {
            var xVelocity = CommonData.ObjRigidbody.velocity.x;
            var acceleration = _GroundedData.MainGrounded ? _Parameters.GroundAcceleration : _Parameters.AirAcceleration;
            xVelocity = Mathf.Lerp(xVelocity, _TargetXVelocity, Time.fixedDeltaTime * acceleration);
            CommonData.ObjRigidbody.velocity = new Vector2(xVelocity, CommonData.ObjRigidbody.velocity.y);
        }

        public override void Update() {
            SetDirection();
            _TargetXVelocity = 0f;
            if (_WalkData.Horizontal > 0.15f)
                _TargetXVelocity = _Parameters.Speed;
            else if (_WalkData.Horizontal < -0.15f)
                _TargetXVelocity = -_Parameters.Speed;
            else
                _WalkData.Horizontal = 0;
        }

        public void SetHorizontal(float hor) {
            _WalkData.Horizontal = hor;
            Mathf.Clamp(_WalkData.Horizontal, -1f, 1f);
        }

        private void SetDirection() {
            if (_WalkData.Horizontal == 0)
                return;
            var newDir = _WalkData.Horizontal > 0 ? 1 : -1;
            CommonData.MovementController.ChangeDirection(newDir);
        }
    }

    [Serializable]
    public class WalkParameters {
        public float Speed = 1f;
        public float GroundAcceleration = 1f;
        public float AirAcceleration = 1f;
        public Transform IkTransform;
    }
}