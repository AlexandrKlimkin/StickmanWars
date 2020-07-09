using Character.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Physics {
    public class Levitation : MonoBehaviour {
        private bool _Levitate = true;
        public float Force;

        public float TorqueForce;
        public float AngularResistance;
        //public float TargetRot;
        public float MaxAngularVelocity;

        public float LevitationEnableGroundDist;
        public float MaxForceDist;
        public float TargetGroundDist;

        public float LinearDrag;
        public float AngularDrag;

        private SimpleRotationController _RotationController;

        [SerializeField]
        private bool _DisableOnDamageTake = true;
        public bool DisableOnDamageTake {
            get {
                return _DisableOnDamageTake;
            }
            set {
                _DisableOnDamageTake = value;
                if (_Damageable == null)
                    return;
                if (value)
                    _Damageable.OnDamage += DisableOnDamage;
                else
                    _Damageable.OnDamage -= DisableOnDamage;
            }
        }

        public bool Rotate = true;

        private float _StartLinearDrag;
        private float _StartAngularDrag;
        private float _RotationStartTimer = 0;

        private Rigidbody2D _Rigidbody;

        private SimpleDamageable _Damageable;

        public void SetActive(bool active) {
            StopAllCoroutines();
            _Levitate = active;
            SwitchLevitation(_Levitate);
            if(!active)
                _RotationController.Rotate = false;
        }

        public void DisableOnTime(float sec) {
            if (sec == 0)
                return;
            StopAllCoroutines();
            StartCoroutine(DisableOnTimeRoutine(sec));
        }


        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
            _Damageable = GetComponent<SimpleDamageable>();
            _RotationController = GetComponentInChildren<SimpleRotationController>();
        }

        private void Start() {
            _StartLinearDrag = _Rigidbody.drag;
            _StartAngularDrag = _Rigidbody.angularDrag;
            if (_Damageable != null && DisableOnDamageTake)
                _Damageable.OnDamage += DisableOnDamage;
        }

        private void FixedUpdate() {
            var leviate = _Levitate;
            //var rot = _Rigidbody.rotation;
            //if (rot < 0)
            //    rot = rot % 360 + 360;
            //if (rot >= 360)
            //    rot = rot % 360;

            var angle = Vector2.SignedAngle(Vector2.up, transform.up);

            var canBerotated = false;
            if (_Levitate) {
                var hit = Physics2D.Raycast(transform.position, Vector2.down, LevitationEnableGroundDist, Layers.Masks.Walkable);
                leviate = hit.collider != null;
                if (hit.collider != null) {
                    _Rigidbody.AddForce(-_Rigidbody.gravityScale * Physics2D.gravity * _Rigidbody.mass); //No gravity
                    var targetPositionY = (hit.point + Vector2.up * TargetGroundDist).y;
                    var dist = targetPositionY - _Rigidbody.position.y;
                    var clampedDist = Mathf.Clamp(dist, -MaxForceDist, MaxForceDist);
                    _Rigidbody.AddForce(Vector2.up * clampedDist * Force);
                    var angularVel = _Rigidbody.angularVelocity;

                    //var rotDelta = rot - TargetRot;
                    _Rigidbody.AddTorque(-angle * TorqueForce);
                    _Rigidbody.AddTorque(-angularVel * AngularResistance);

                    var rotatedCorrect = Mathf.Abs(angle) < 10f;
                    var lowAngularVelocity = Mathf.Abs(_Rigidbody.angularVelocity) < 10f;
                    var lowVelocity = _Rigidbody.velocity.sqrMagnitude < 5f;
                    var goodHeight = dist < 1;
                    canBerotated = _Levitate && rotatedCorrect && lowAngularVelocity && goodHeight && lowVelocity;
                }
            }
            if (_RotationController != null) {
                if (canBerotated)
                    _RotationStartTimer += Time.fixedDeltaTime;
                else
                    _RotationStartTimer = 0;
                _RotationController.Rotate = _RotationStartTimer >= 1f;
            }
            SwitchLevitation(leviate);
        }

        private void DisableOnDamage(SimpleDamageable dmgbl, Damage damage) {
            DisableOnTime(6f);
        }

        private void SwitchLevitation(bool leviate) {
            _Rigidbody.angularDrag = leviate ? AngularDrag : _StartAngularDrag;
            _Rigidbody.drag = leviate ? LinearDrag : _StartLinearDrag;
        }


        private float _TimeToEnable;
        private IEnumerator DisableOnTimeRoutine(float sec) {
            _Levitate = false;
            SwitchLevitation(_Levitate);
            yield return new WaitForSeconds(sec);
            _Levitate = true;
            SwitchLevitation(_Levitate);
        }

        private void OnDestroy() {
            if(_Damageable != null)
                _Damageable.OnDamage -= DisableOnDamage;
        }
    }
}
