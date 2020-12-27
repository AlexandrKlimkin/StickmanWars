using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.Movement.Modules {
    public class OneWayPlatformModule : MovementModule {

        private GroundedData _GroundedData;
        private WalkData _WalkData;

        private OneWayPlatformParameters _Parameters;

        private List<Collider2D> _FallingDownColliders;
        private float _FallingDownCollidersTime;

        public OneWayPlatformModule(OneWayPlatformParameters Parameters) {
            this._Parameters = Parameters;
        }

        public override void Start() {
            _GroundedData = BB.Get<GroundedData>();
            _WalkData = BB.Get<WalkData>();
        }

        public override void Update() {
            base.Update();
            if (!_FallingDownColliders.IsNullOrEmpty()) {
                if(Time.time >= _FallingDownCollidersTime) {
                    _FallingDownColliders.ForEach(_ => { Physics2D.IgnoreCollision(_, CommonData.BodyCollider, false); });
                    _FallingDownColliders.ForEach(_ => { Physics2D.IgnoreCollision(_, CommonData.GroundCollider, false); });
                    _FallingDownColliders = null;
                }
            }
        }

        public bool FallDownPlatform() {
            if (!_GroundedData.GroundedEffector)
                return false;
            if (_WalkData.Vertical > -0.2f)
                return false;
            if (_GroundedData.TouchedEffectorColliders.IsNullOrEmpty())
                return false;
            _FallingDownColliders = _GroundedData.TouchedEffectorColliders;
            _FallingDownCollidersTime = Time.time + _Parameters.PlatformIgnoreCollisionTime;
            _FallingDownColliders.ForEach(_ => { Physics2D.IgnoreCollision(_, CommonData.BodyCollider); });
            _FallingDownColliders.ForEach(_ => { Physics2D.IgnoreCollision(_, CommonData.GroundCollider); });
            return true;
        }
    }

    [Serializable]
    public class OneWayPlatformParameters {
        public float PlatformIgnoreCollisionTime;
    }
}
