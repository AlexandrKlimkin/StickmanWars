using System;
using UnityEngine;

namespace Character.Movement.Modules
{
    public class WallsSlideModule : MovementModule
    {
        public bool WallSliding => _WallSlideData.WallSliding;

        private WallsSlideParameters _Parameters;
        private WallSlideData _WallSlideData;
        private GroundedData _GroundedData;

        public WallsSlideModule(WallsSlideParameters parameters)
        {
            _Parameters = parameters;
        }

        public override void Start()
        {
            _WallSlideData = BB.Get<WallSlideData>();
            _GroundedData = BB.Get<GroundedData>();
        }

        public override void Update()
        {
            _WallSlideData.WallSliding = !_GroundedData.MainGrounded && (_WallSlideData.LeftTouch || _WallSlideData.RightTouch);
            if (_WallSlideData.WallSliding)
            {
                if (CommonData.ObjRigidbody.velocity.y < -_Parameters.WallSlideSpeed)
                    CommonData.ObjRigidbody.velocity = new Vector2(CommonData.ObjRigidbody.velocity.x, -_Parameters.WallSlideSpeed);
            }
        }
    }
}

[Serializable]
public class WallsSlideParameters
{
    public float WallSlideSpeed;
}