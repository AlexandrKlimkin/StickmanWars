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
        public float StartSpeed;

        //private Vector2 _LastTargetPos;

        private Rigidbody2D _Rigidbody;
        private Transform _Target;
        private float _Timer;

        private ContactFilter2D _Filter = new ContactFilter2D() { useTriggers = false };

        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
        }

        private bool _FirstSimulate;

        public override void Simulate(float time) {
            if (_FirstSimulate) {
                _FirstSimulate = false;
                _Rigidbody.velocity = transform.forward * StartSpeed;
            }
            _Timer += time;
            var activated = _Timer >= ActivationTime;
            if (!activated) {
                transform.rotation = Quaternion.LookRotation(_Rigidbody.velocity);
                return;
            }
            if (!_Target)
                FindTarget();
            else {
                var targetVector = (_Target.position - transform.position);
                var targetRotation = Quaternion.LookRotation(targetVector);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, time * GuidanceSmoothness);
            }
            _Rigidbody.simulated = false;
            var nextPos = transform.position + transform.forward * Data.Speed * time;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            var hitsCount = Physics2D.Linecast(transform.position, nextPos, _Filter, results);
            var hit = results.FirstOrDefault();
            transform.position = (hitsCount > 0 && hit.transform) ? (Vector3) hit.point : nextPos;
            if (hit.transform != null) {
                PerformHit(hit.transform.GetComponent<IDamageable>());
            }
        }

        public override void Setup(RocketProjectileData data) {
            base.Setup(data);
            _Timer = 0;
            _Target = null;
            _Rigidbody.simulated = true;
            _FirstSimulate = true;
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
