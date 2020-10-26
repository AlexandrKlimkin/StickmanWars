using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Tools.VisualEffects;
using UnityEngine;
using System.Linq;

namespace Character.Shooting {
    public class BulletProjectile : Projectile<BulletProjectileData> {

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
    }
}