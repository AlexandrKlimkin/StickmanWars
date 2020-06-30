using System;
using UnityEngine;

namespace Character.Movement.Modules {
    public class WallsSlideModule : MovementModule {
        public bool WallSliding => _WallSlideData.WallSliding;
        public bool WallRun => _WallSlideData.WallRun;

        private WallsSlideParameters _Parameters;
        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;

        public WallsSlideModule(WallsSlideParameters parameters) {
            _Parameters = parameters;
        }

        public override void Start() {
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
        }

        public override void Update() {
            _WallSlideData.WallRun = !_GroundedData.MainGrounded && (_WallSlideData.LeftTouch || _WallSlideData.RightTouch) && !_GroundedData.FallingDown && !_WallSlideData.LedgeHanging;
            _WallSlideData.WallSliding = !_GroundedData.MainGrounded && (_WallSlideData.LeftTouch || _WallSlideData.RightTouch) && _GroundedData.FallingDown && !_WallSlideData.WallRun && !_WallSlideData.LedgeHanging;
            if (!_WallSlideData.WallSliding)
                return;
            SetDirection();
            if (CommonData.ObjRigidbody.velocity.y < -_Parameters.WallSlideSpeed)
                CommonData.ObjRigidbody.velocity = new Vector2(CommonData.ObjRigidbody.velocity.x, -_Parameters.WallSlideSpeed);
        }

        private void SetDirection() {
            var newDir = 1;
            if (_WallSlideData.LeftTouch)
                newDir = -1;
            else if (_WallSlideData.RightTouch)
                newDir = 1;
            CommonData.MovementController.ChangeDirection(newDir);
        }
    }
}

[Serializable]
public class WallsSlideParameters {
    public float WallSlideSpeed;
}