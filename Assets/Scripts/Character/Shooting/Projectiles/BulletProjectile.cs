using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

namespace Character.Shooting
{
    public class BulletProjectile : Projectile<ProjectileData>
    {
        public GameObject HitEffect;

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

        protected override void PerformHit(IDamageable damageable)
        {
            base.PerformHit(damageable);
            if (HitEffect == null)
                return;
            var effect = Instantiate(HitEffect);
            effect.transform.position = transform.position;
        }
    }
}