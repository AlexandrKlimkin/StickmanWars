using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;
using System.Linq;

namespace Character.Shooting {
    public class BulletProjectile : Projectile<BulletProjectileData> {
        public Transform TrailTransformOverride;
        public string TrailName;

        protected AttachedParticleEffect _Trail;

        private ContactFilter2D _Filter;

        public override void Simulate(float time) {
            var targetPos = transform.position + transform.forward * Data.Speed * time;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            var hitsCount = Physics2D.Linecast(transform.position, targetPos, _Filter, results);
            var hit = results.FirstOrDefault();
            transform.position = (hitsCount > 0 && hit.transform) ? (Vector3)hit.point : targetPos;
            if (hit.transform != null) {
                var damageable = hit.transform.GetComponent<IDamageable>();
                PerformHit(damageable);
                //var effect = DintEffect(hit.point, hit.normal);
                //effect.transform.SetParent(hit.transform);
            }
        }

        protected override void Initialize() {
            base.Initialize();
            AttachTrail();
            _Filter = new ContactFilter2D() {
                useTriggers = false,
                layerMask = Layers.Masks.ForProjectiles,
                useLayerMask = true
            };
        }

        protected override void PerformHit(IDamageable damageable, bool killProjectile = true) {
            var force = new Vector2(transform.forward.x, transform.forward.y) * Data.Force;
            damageable?.Collider?.attachedRigidbody?.AddForceAtPosition(force, transform.position);
            //if (damageable == null)
            //    return;
            //var collider = damageable.Collider;
            //var attachedRb = collider.attachedRigidbody;
            //var force = Data.Force;
            //attachedRb.AddForceAtPosition(new Vector2(transform.forward.x, transform.forward.y) * force, transform.position);
            //PlayHitEffect();
            Data.Damage.DamagePos = transform.position;
            Data.Damage.DamageForce = force * 10f;
            base.PerformHit(damageable, killProjectile);
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

        protected override void KillProjectile() {
            base.KillProjectile();
            DetachTrail();
        }

    }
}