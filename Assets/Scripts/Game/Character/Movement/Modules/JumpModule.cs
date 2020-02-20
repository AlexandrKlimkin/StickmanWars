using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Movement.Modules
{
    public class JumpModule : MovementModule
    {
        private JumpParameters _Parameters;

        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;

        private float _JumpTimer;

        public JumpModule(JumpParameters parameters)
        {
            _Parameters = parameters;
        }

        public override void Start()
        {
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
        }

        public bool Jump(MonoBehaviour behaviour) {
            if (_GroundedData.Grounded && _GroundedData.TimeSinceMainGrounded < 0.3f) {
                _JumpTimer = _Parameters.LowJumpTime;
                behaviour.StopCoroutine(JumpRoutine());
                behaviour.StartCoroutine(JumpRoutine());
                return true;
            }
            return false;
        }

        private IEnumerator JumpRoutine() {
            var gravityScale = CommonData.ObjRigidbody.gravityScale;
            while (_JumpTimer > 0) {
                CommonData.ObjRigidbody.velocity = new Vector2(CommonData.ObjRigidbody.velocity.x, _Parameters.JumpSpeed);
                CommonData.ObjRigidbody.gravityScale = 0;
                _JumpTimer -= Time.deltaTime;
                yield return null;
            }
            CommonData.ObjRigidbody.gravityScale = gravityScale;
            _JumpTimer = 0;
        }

        public void ContinueJump() {
            if (_JumpTimer != 0)
                _JumpTimer += _Parameters.HighJumpAddTime;
        }

        public bool WallJump() {
            if (_WallSlideData.RightTouch) {
                var vector = new Vector2(-1, 1).normalized;
                CommonData.ObjRigidbody.velocity = vector * _Parameters.WallJumpSpeed;
                return true;
            }
            if (_WallSlideData.LeftTouch) {
                var vector = new Vector2(1, 1).normalized;
                CommonData.ObjRigidbody.velocity = vector * _Parameters.WallJumpSpeed;
                return true;
            }
            return false;
        }

        public void ContinueWallJump() {
            if (_JumpTimer != 0)
                _JumpTimer += _Parameters.HighJumpAddTime;
        }
    }

    [Serializable]
    public class JumpParameters
    {
        public float JumpSpeed;
        public float WallJumpSpeed;
        public float LowJumpTime;
        public float HighJumpAddTime;
    }
}