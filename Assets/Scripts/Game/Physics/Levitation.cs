using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Physics {
    public class Levitation : MonoBehaviour {
        public bool Levitate;
        public float Force;
        public float AngularVelocityDamping;
        public float MaxLeviateAngularVelocity;
        private Rigidbody2D _Rigidbody;

        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            if(!Levitate)
                return;
            _Rigidbody.AddForce(-_Rigidbody.gravityScale * Physics2D.gravity * Force * _Rigidbody.mass);

            var zRot = _Rigidbody.rotation;
            var targetZRot = 0;
            var delta = targetZRot - zRot;

            var targetAngularVelocity = delta / 180 * MaxLeviateAngularVelocity;

            _Rigidbody.angularVelocity = Mathf.Lerp(_Rigidbody.angularVelocity, targetAngularVelocity, Time.fixedDeltaTime * AngularVelocityDamping);

            //_Rigidbody.angularVelocity += delta * AngularVelocityDamping;

            //_Rigidbody.angularVelocity = Mathf.Lerp(_Rigidbody.angularVelocity, 0, Time.fixedDeltaTime * AngularVelocityDamping);
        }
    }
}
