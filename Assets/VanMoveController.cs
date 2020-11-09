using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class VanMoveController : MonoBehaviour {

        private VanMoveParameters _Parameters;
        private Rigidbody2D _Rigidbody;
        private float _Timer;
        private TrainState _TrainState = TrainState.NotMoving;

        void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetParameters(VanMoveParameters parameters) {
            _Parameters = parameters;
        }

        private void FixedUpdate() {
            if(_Parameters == null)
                return;
            if (_TrainState == TrainState.NotMoving)
                NotMovingStateUpdate();
            else if (_TrainState == TrainState.Accelerating)
                AcceleratingStateUpdate();
            else if (_TrainState == TrainState.Moving)
                MovingStateUpdate();
            _Timer += Time.fixedDeltaTime;
        }

        private void NotMovingStateUpdate() {
            if (_Timer >= _Parameters.Delay)
                _TrainState = TrainState.Accelerating;
        }

        private void AcceleratingStateUpdate() {
            if (_Rigidbody.velocity.x < _Parameters.Velocity.x) {
                _Rigidbody.velocity += _Parameters.Velocity.normalized * _Parameters.Acceleration * Time.deltaTime;
            }
            else {
                _Rigidbody.velocity = Vector2.ClampMagnitude(_Rigidbody.velocity, _Parameters.Velocity.magnitude);
                _TrainState = TrainState.Moving;
            }
        }

        private void MovingStateUpdate() {
            if (!_Rotate && _Rigidbody.position.x >= _Parameters.StartRotationTransform.position.x) {
                _StartRotationTime = Time.time;
                _Rotate = true;
            }
            if (_StartRotationTime != 0) {
                var rotateTime = Time.time - _StartRotationTime;
                var normilizedRotateTime = Mathf.Clamp01(rotateTime / _Parameters.RotationTime);
                var curveValue = _Parameters.FallingRotationCurve.Evaluate(normilizedRotateTime);
                var angularVelocity = curveValue * _Parameters.MaxAngularSpeed;
                _Rigidbody.angularVelocity = angularVelocity;
            }
            if (_Rigidbody.position.x >= _Parameters.StartGravityTransform.position.x) {
                _Rigidbody.velocity += Vector2.down * _Parameters.GravityAcceleration * Time.fixedDeltaTime;
            }
        }

        private float _StartRotationTime = 0;
        private bool _Rotate = false;

        private enum TrainState {
            NotMoving,
            Accelerating,
            Moving,
        }
    }
}
