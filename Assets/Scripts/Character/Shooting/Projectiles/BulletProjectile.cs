﻿using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting
{
    public class BulletProjectile : Projectile<ProjectileData>
    {
        public string HitEffectName;
        public string TrailName;

        protected TrailEffect _Trail;

        public override void Simulate(float time)
        {
            var targetPos = transform.position + transform.forward * Data.Speed * time;
            var hit = Physics2D.Linecast(transform.position, targetPos);
            transform.position = hit.transform ? (Vector3)hit.point : targetPos;
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

        protected override void PerformHit(IDamageable damageable)
        {
            base.PerformHit(damageable);
            PlayHitEffect();
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