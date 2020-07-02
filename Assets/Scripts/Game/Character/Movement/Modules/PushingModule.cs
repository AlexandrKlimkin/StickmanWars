using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.Movement.Modules {
    public class PushingModule : MovementModule {

        public bool Pushing => Mathf.Abs(_CommonData.MovementController.Velocity.x) > 0.1f && _GroundedData.Grounded && (_Parameters.RightSensor.IsTouching || _Parameters.LeftSensor.IsTouching);

        private PushingParameters _Parameters;

        private GroundedData _GroundedData;
        private CommonData _CommonData;

        public PushingModule(PushingParameters parameters) {
            _Parameters = parameters;
        }

        public override void Start() {
            _GroundedData = BB.Get<GroundedData>();
            _CommonData = BB.Get<CommonData>();
        }

    }

    [Serializable]
    public class PushingParameters {
        public Sensor RightSensor;
        public Sensor LeftSensor;
    }
}
