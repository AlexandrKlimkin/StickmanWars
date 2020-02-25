using System;
using System.Linq;
using UnityEngine;

namespace Character.Movement.Modules
{
    public class LedgeHangModule : MovementModule
    {
        public bool LedgeHang => _WallSlideData.LedgeHanging;

        private LedgeHangParameters _Parameters;

        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;
        private JumpData _JumpData;

        private float _StartGravityScale;

        public LedgeHangModule(LedgeHangParameters parameters)
        {
            _Parameters = parameters;
        }

        public override void Start() {
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
            _JumpData = BB.Get<JumpData>();
            _StartGravityScale = CommonData.ObjRigidbody.gravityScale;
        }

        public override void Update()
        {
            var timeSinceLastJump = Time.time - _JumpData.LastWallJumpTime;
            _WallSlideData.LedgeHanging = !_GroundedData.MainGrounded && 
                                          timeSinceLastJump > 0.2f &&
                                          (_WallSlideData.LeftTouch || _WallSlideData.RightTouch) &&
                                          _Parameters.UpSensor.IsTouching;
            if (_WallSlideData.LedgeHanging)
            {
                var firstTrigger = _Parameters.UpSensor.TouchedColliders.FirstOrDefault(_ => _.GetComponent(typeof(WallHangTrigger)));
                if (firstTrigger != null)
                {
                    var posY = firstTrigger.transform.position.y;
                    var posX = firstTrigger.transform.position.x;
                    var deltaY = _Parameters.UpSensor.transform.position.y - CommonData.ObjTransform.position.y;
                    CommonData.ObjRigidbody.position = new Vector2(posX, posY - deltaY);
                }
                CommonData.ObjRigidbody.gravityScale = 0;
                CommonData.ObjRigidbody.velocity = Vector2.zero;
            }
            else
            {
                CommonData.ObjRigidbody.gravityScale = _StartGravityScale;
            }
        }
    }

    [Serializable]
    public class LedgeHangParameters
    {
        public Sensor UpSensor;
    }
}