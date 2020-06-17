using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Movement.Modules
{
    public class GroundCheckModule : MovementModule
    {
        public bool IsGrounded => _GroundedData.Grounded;
        public bool IsMainGrounded => _GroundedData.MainGrounded;
        public float MinDistanceToGround => _GroundedData.MinDistanceToGround;
        public bool FallingDown => _GroundedData.FallingDown;

        private GroundedData _GroundedData;
        private WallSlideData _WallSlideData;

        private GroundCheckParameters _Parameters;
        private float _LastY;

        public GroundCheckModule(GroundCheckParameters parameters)
        {
            _Parameters = parameters;
        }

        public override void Start() {
            _GroundedData = BB.Get<GroundedData>();
            _WallSlideData = BB.Get<WallSlideData>();
            _LastY = CommonData.ObjTransform.position.y;
        }

        public override void Update()
        {
            _GroundedData.Grounded = _Parameters.GroundSensors.Any(_ => _.IsTouching) && !_WallSlideData.WallSliding;
            _GroundedData.MainGrounded = _Parameters.MainGroundSensors.Any(_ => _.IsTouching);/* && _Parameters.MainGroundSensor.Distanse < 1f*/;
            _GroundedData.FallingDown = CommonData.ObjTransform.position.y < _LastY && !_GroundedData.MainGrounded;
            _GroundedData.MinDistanceToGround = _Parameters.GroundSensors.Min(_ => _.Distanse);
            if (_GroundedData.MainGrounded)
                _GroundedData.TimeSinceMainGrounded = 0f;
            else
                _GroundedData.TimeSinceMainGrounded += Time.deltaTime;
            _LastY = CommonData.ObjTransform.position.y;
        }
    }

    [Serializable]
    public class GroundCheckParameters
    {
        public List<Sensor> GroundSensors;
        public List<Sensor> MainGroundSensors;
    }
}