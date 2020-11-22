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
        public float GuidanceSmoothness;
        public float TargetSearchRadius;
        public float Gravity;
        public float StartSpeed;
        public float TargetAimLerpSmoothness;
        public float RotationSmoothness;

        private Transform _Target;
        private float _Timer;

        private Vector3 _Velocity;

        private ContactFilter2D _Filter = new ContactFilter2D() { useTriggers = false };

        private void Awake() {

        }

        public override void Simulate(float time) {
            _Timer += time;
            var activated = _Timer >= ActivationTime;
            if (!activated) {
                _Velocity += Vector3.down * Gravity * time;
            } else {
                if (!_Target)
                    FindTarget();
                else {
                    var targetVector = (_Target.position - transform.position);
                    var velMagnitude = Data.Speed;
                    var velNormilized = _Velocity.normalized;
                    var targetNormilized = targetVector.normalized;
                    velNormilized = Vector3.Lerp(velNormilized, targetNormilized, TargetAimLerpSmoothness);
                    _Velocity = velNormilized * velMagnitude;
                }
            }
            var nextPos = transform.position + _Velocity * time;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            var hitsCount = Physics2D.Linecast(transform.position, nextPos, _Filter, results);
            var hit = results.FirstOrDefault();

            transform.position = (hitsCount > 0 && hit.transform) ? (Vector3) hit.point : nextPos;

            var targetRotation = Quaternion.LookRotation(_Velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, time * RotationSmoothness);

            if (hit.transform != null) {
                PerformHit(hit.transform.GetComponent<IDamageable>());
            }
        }

        public override void Setup(RocketProjectileData data) {
            base.Setup(data);
            _Timer = 0;
            _Target = null;
            _Velocity = transform.forward * StartSpeed;
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
