using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Physics {
    public class Levitation : MonoBehaviour {
        public bool Levitate;
        public float Force;
        public float TorqueForce;
        public float AngularResistance;
        public float TargetRot;
        public float MaxAngularVelocity;

        public float LevitationEnableGroundDist;
        public float TargetGroundDist;

        private Rigidbody2D _Rigidbody;

        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            if(!Levitate)
                return;
            var hit = Physics2D.Raycast(transform.position, Vector2.down, LevitationEnableGroundDist, Layers.Masks.Walkable);
            if(hit.collider == null) {

            }
            else {
                _Rigidbody.AddForce(-_Rigidbody.gravityScale * Physics2D.gravity * _Rigidbody.mass);
                var targetPositionY = (hit.point + Vector2.up * TargetGroundDist).y;
                _Rigidbody.AddForce(Vector2.up * (targetPositionY - _Rigidbody.position.y) * Force);
                var angularVel = _Rigidbody.angularVelocity;
                var rot = _Rigidbody.rotation;
                var rotDelta = rot - TargetRot;
                _Rigidbody.AddTorque(-rotDelta * TorqueForce);
                _Rigidbody.AddTorque(-angularVel * AngularResistance);
            }
        }
    }
}
