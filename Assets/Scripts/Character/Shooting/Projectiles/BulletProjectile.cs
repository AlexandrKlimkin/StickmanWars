using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stickman.Shooting
{
    public class BulletProjectile : Projectile<ProjectileData>
    {
        public GameObject HitEffect;

        public override void Simulate(float time)
        {
            var targetPos = transform.position + transform.forward * Data.Speed * time;
            var hit = Physics2D.Linecast(transform.position, targetPos);
            transform.position = hit.transform ? (Vector3)hit.point : targetPos;
            if(hit.transform != null)
                PerformHit();
        }

        protected override void PerformHit()
        {
            base.PerformHit();
            if (HitEffect != null)
            {
                var effect = Instantiate(HitEffect);
                effect.transform.position = transform.position;
            }
        }
    }
}