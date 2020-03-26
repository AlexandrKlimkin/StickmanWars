using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;
using System.Linq;

namespace Character.Shooting
{
    public class BulletProjectile : Projectile<BulletProjectileData>
    {
        public string HitEffectName;
        public string TrailName;

        protected TrailEffect _Trail;

        private ContactFilter2D _Filter = new ContactFilter2D() { useTriggers = false };

        public override void Simulate(float time)
        {
            var targetPos = transform.position + transform.forward * Data.Speed * time;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            var hitsCount = Physics2D.Linecast(transform.position, targetPos, _Filter, results);
            var hit = results.FirstOrDefault();
            transform.position = (hitsCount > 0 && hit.transform) ? (Vector3)hit.point : targetPos;
            if (hit.transform != null)
            {
                PerformHit(hit.transform.GetComponent<IDamageable>());
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            AttachTrail();
        }

        protected override void PerformHit(IDamageable damageable, bool killProjectile = true)
        {
            base.PerformHit(damageable, killProjectile);
            Data.Damage.Force = transform.right;
            PlayHitEffect();
            damageable?.Collider?.attachedRigidbody?.AddForceAtPosition(new Vector2(transform.forward.x, transform.forward.y) * Data.Force, transform.position);
        }

        protected virtual void AttachTrail()
        {
            if (TrailName == null)
                return;
            _Trail = VisualEffect.GetEffect<TrailEffect>(TrailName);
            _Trail.Attach(this.transform);
            _Trail.Play();
        }

        protected virtual void DetachTrail()
        {
            _Trail.Detach();
            _Trail = null;
        }

        protected virtual void PlayHitEffect()
        {
            if (HitEffectName != null)
            {
                var effect = VisualEffect.GetEffect<ParticleEffect>(HitEffectName);
                effect.transform.position = transform.position;
                effect.Play();
            }
        }

        protected override void KillProjectile()
        {
            base.KillProjectile();
            DetachTrail();
        }

    }
}