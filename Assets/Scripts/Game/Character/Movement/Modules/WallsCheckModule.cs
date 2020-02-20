using System;
using System.Collections.Generic;
using System.Linq;

namespace Character.Movement.Modules
{
    public class WallsCheckModule : MovementModule
    {
        private WallSlideData _WallSlideData;

        private WallCheckParameters _Parameters;

        public WallsCheckModule(WallCheckParameters Parameters) : base() {
            this._Parameters = Parameters;
        }

        public override void Start() {
            _WallSlideData = BB.Get<WallSlideData>();
        }

        public override void Update()
        {
            _WallSlideData.LeftTouch = _Parameters.LeftSensors.Any(_ => _.IsTouching);
            _WallSlideData.RightTouch = _Parameters.RightSensors.Any(_ => _.IsTouching);
        }
    }

    [Serializable]
    public class WallCheckParameters {
        public List<Sensor> RightSensors;
        public List<Sensor> LeftSensors;
    }
}