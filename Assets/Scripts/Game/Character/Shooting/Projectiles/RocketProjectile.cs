using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Character.Health;
using Character.Shooting;
using UnityEngine;

namespace Character.Shooting {
    public class RocketProjectile : Projectile<RocketProjectileData> {
        public float ActivationTime;
        public float TargetSearchRadius;
        public float Gravity;

        public Vector2 TargetAimLerpSmoothnessVector;
        public AnimationCurve TargetAimLerpSmoothnessCurve;
        public Vector2 NotActivatedSpeedVector;
        public AnimationCurve NotActivatedSpeedCurve;
        public Vector2 ActivatedSpeedVector;
        public float ActivatedAccelerationTime;
        public AnimationCurve ActivatedSpeedCurve;

        private Transform _Target;
        private float _Timer;
        private float _ActivatedTimer;

        private Vector3 _Velocity;

        private ContactFilter2D _Filter = new ContactFilter2D() { useTriggers = false };

        private void Awake() {

        }

        public override void Simulate(float time) {
            _Timer += time;
            var activated = _Timer >= ActivationTime;
            if (!activated) {
                var timeFraction = (_Timer / ActivationTime);
                var velocityFraction = NotActivatedSpeedCurve.Evaluate(timeFraction);
                var velocityMagnitude = Mathf.Lerp(NotActivatedSpeedVector.x, NotActivatedSpeedVector.y, velocityFraction);
                _Velocity += Vector3.down * Gravity * time;
                _Velocity = _Velocity.normalized * velocityMagnitude;
            } else {
                if (!_Target)
                    FindTarget();
                else {
                    var targetVector = (_Target.position - transform.position);
                    var activatedFraction = (_Timer - ActivationTime) / ActivatedAccelerationTime;
                    var velMagnitude = Mathf.Lerp(ActivatedSpeedVector.x, ActivatedSpeedVector.y, activatedFraction);
                    var targetAimSmoothness = Mathf.Lerp(TargetAimLerpSmoothnessVector.x, TargetAimLerpSmoothnessVector.y, activatedFraction);
                    var velNormilized = _Velocity.normalized;
                    var targetNormilized = targetVector.normalized;
                    velNormilized = Vector3.Lerp(velNormilized, targetNormilized, targetAimSmoothness);
                    _Velocity = velNormilized * velMagnitude;
                }
            }
            var nextPos = transform.position + _Velocity * time;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            var hitsCount = Physics2D.Linecast(transform.position, nextPos, _Filter, results);
            var hit = results.FirstOrDefault();

            transform.position = (hitsCount > 0 && hit.transform) ? (Vector3) hit.point : nextPos;
            var rot = transform.rotation.eulerAngles;
            var targetRotation = Quaternion.LookRotation(_Velocity, Vector3.up);
            //var targetEuler = targetRotation.eulerAngles;
            //var rotX = transform.rotation.eulerAngles.x;
            //var newRotX = Mathf.Lerp(rotX, targetEuler.x, time * RotationSmoothness);
            transform.rotation = targetRotation;//Quaternion.Euler(newRotX, targetEuler.y, targetEuler.z);
            if (hit.transform != null) {
                PerformHit(hit.transform.GetComponent<IDamageable>());
            }
        }

        public override void Setup(RocketProjectileData data) {
            base.Setup(data);
            _Timer = 0;
            _Target = null;
            var velMagnitude = 0f;
            if(ActivationTime > 0) {
                var fraction = Mathf.Lerp(NotActivatedSpeedVector.x, NotActivatedSpeedVector.y, 0);
                velMagnitude = NotActivatedSpeedCurve.Evaluate(fraction);
            } else {
                var fraction = Mathf.Lerp(ActivatedSpeedVector.x, ActivatedSpeedVector.y, 0);
                velMagnitude = ActivatedSpeedCurve.Evaluate(fraction);
            }
            _Velocity = transform.forward * velMagnitude;
        }

        private void FindTarget() {
            var units = CharacterUnit.Characters.Where(_=>_ && _.OwnerId != Data.OwnerId).ToList();
            if (units.Count <= 0) {
                _Target = null;
                return;
            }
            var unitPositions = units.Select(_ => _.transform.position.ToVector2()).ToList();
            var closestIndex = Utils.GetClosestIndex(unitPositions, transform.position.ToVector2());
            var closestUnit = units[closestIndex];
            _Target = closestUnit.transform;
        }

        protected override void ApplyDamage(IDamageable damageable, Damage dmg) {

        }
    }
}
