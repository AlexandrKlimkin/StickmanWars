using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Physics {
    public class Levitation : MonoBehaviour {
        private bool _Levitate = true;
        public float Force;

        public float TorqueForce;
        public float AngularResistance;
        public float TargetRot;
        public float MaxAngularVelocity;

        public float LevitationEnableGroundDist;
        public float MaxForceDist;
        public float TargetGroundDist;

        public float LinearDrag;
        public float AngularDrag;

        private float _StartLinearDrag;
        private float _StartAngularDrag;

        private Rigidbody2D _Rigidbody;

        public void SetActive(bool active) {
            StopAllCoroutines();
            _Levitate = active;
            SwitchLevitation(_Levitate);
        }

        public void DisableOnTime(float sec) {
            if (sec == 0)
                return;
            StopAllCoroutines();
            StartCoroutine(DisableOnTimeRoutine(sec));
        }


        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            _StartLinearDrag = _Rigidbody.drag;
            _StartAngularDrag = _Rigidbody.angularDrag;
        }

        private void FixedUpdate() {
            var leviate = _Levitate;
            if (!_Levitate) {
                SwitchLevitation(leviate);
                return;
            }
            var hit = Physics2D.Raycast(transform.position, Vector2.down, LevitationEnableGroundDist, Layers.Masks.Walkable);
            leviate = hit.collider != null;
            if (hit.collider != null) {
                _Rigidbody.AddForce(-_Rigidbody.gravityScale * Physics2D.gravity * _Rigidbody.mass); //No gravity
                var targetPositionY = (hit.point + Vector2.up * TargetGroundDist).y;
                var dist = Mathf.Clamp(targetPositionY - _Rigidbody.position.y, -MaxForceDist, MaxForceDist);
                _Rigidbody.AddForce(Vector2.up * dist * Force);
                var angularVel = _Rigidbody.angularVelocity;
                var rot = _Rigidbody.rotation;
                var rotDelta = rot - TargetRot;
                _Rigidbody.AddTorque(-rotDelta * TorqueForce);
                _Rigidbody.AddTorque(-angularVel * AngularResistance);
            }
            SwitchLevitation(leviate);
        }

        private void SwitchLevitation(bool leviate) {
            _Rigidbody.angularDrag = leviate ? AngularDrag : _StartAngularDrag;
            _Rigidbody.drag = leviate ? LinearDrag : _StartLinearDrag;
        }


        private IEnumerator DisableOnTimeRoutine(float sec) {
            _Levitate = false;
            SwitchLevitation(_Levitate);
            yield return new WaitForSeconds(sec);
            _Levitate = true;
            SwitchLevitation(_Levitate);
        }
    }
}
