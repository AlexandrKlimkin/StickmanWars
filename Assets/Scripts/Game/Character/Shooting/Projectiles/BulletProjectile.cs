using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;
using System.Linq;

namespace Character.Shooting {
    public class BulletProjectile : Projectile<BulletProjectileData> {
        public List<string> HitEffectNames;
        public Transform TrailTransformOverride;
        public string TrailName;

        protected AttachedParticleEffect _Trail;

        private ContactFilter2D _Filter = new ContactFilter2D() { useTriggers = false };

        public override void Simulate(float time) {
            var targetPos = transform.position + transform.forward * Data.Speed * time;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            var hitsCount = Physics2D.Linecast(transform.position, targetPos, _Filter, results);
            var hit = results.FirstOrDefault();
            transform.position = (hitsCount > 0 && hit.transform) ? (Vector3)hit.point : targetPos;
            if (hit.transform != null) {
                PerformHit(hit.transform.GetComponent<IDamageable>());
            }
        }

        protected override void Initialize() {
            base.Initialize();
            AttachTrail();
        }

        protected override void PerformHit(IDamageable damageable, bool killProjectile = true) {
            base.PerformHit(damageable, killProjectile);
            PlayHitEffect();
            damageable?.Collider?.attachedRigidbody?.AddForceAtPosition(new Vector2(transform.forward.x, transform.forward.y) * Data.Force, transform.position);
        }

        protected virtual void AttachTrail() {
            if (string.IsNullOrEmpty(TrailName))
                return;
            _Trail = GetEffect<AttachedParticleEffect>(TrailName);
            _Trail.gameObject.SetActive(true);
            var target = TrailTransformOverride ?? this.transform;
            _Trail.SetTarget(target);
            _Trail.Play();
        }

        protected virtual void DetachTrail() {
            if (_Trail == null)
                return;
            //_Trail.SetTarget(null);
            _Trail = null;
        }

        protected virtual void PlayHitEffect() {
            if (HitEffectNames != null && HitEffectNames.Count > 0) {
                var randIndex = Random.Range(0, HitEffectNames.Count);
                var effect = GetEffect<ParticleEffect>(HitEffectNames[randIndex]);
                effect.transform.position = transform.position;
                effect.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
                effect.Play();
            }
        }

        protected override void KillProjectile() {
            base.KillProjectile();
            DetachTrail();
        }

    }
}