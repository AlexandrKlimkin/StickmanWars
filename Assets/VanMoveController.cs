using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class VanMoveController : MonoBehaviour {
        private VanMoveParameters _Parameters;
        public Rigidbody2D Rigidbody { get; private set; }
        public SimpleDamageable SimpleDamageable { get; private set; }

        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            SimpleDamageable = GetComponent<SimpleDamageable>();
        }

        public void SetParameters(VanMoveParameters parameters) {
            _Parameters = parameters;
            MovingUpdate();
        }

        private void FixedUpdate() {
            MovingUpdate();
        }

        private void MovingUpdate() {
            if (_Parameters == null)
                return;
            if (!_Rotate && Rigidbody.position.x >= _Parameters.StartRotationTransform.position.x) {
                _StartRotationTime = Time.time;
                _Rotate = true;
            }
            if (_StartRotationTime != 0) {
                var rotateTime = Time.time - _StartRotationTime;
                var normilizedRotateTime = Mathf.Clamp01(rotateTime / _Parameters.RotationTime);
                var curveValue = _Parameters.FallingRotationCurve.Evaluate(normilizedRotateTime);
                var angularVelocity = curveValue * _Parameters.MaxAngularSpeed;
                Rigidbody.angularVelocity = angularVelocity;
            }
            if (Rigidbody.position.x >= _Parameters.StartGravityTransform.position.x) {
                Rigidbody.velocity += Vector2.down * _Parameters.GravityAcceleration * Time.fixedDeltaTime;
            }
            else {
                Rigidbody.velocity = _Parameters.Velocity;
            }
        }

        private float _StartRotationTime = 0;
        private bool _Rotate = false;

    }
}
