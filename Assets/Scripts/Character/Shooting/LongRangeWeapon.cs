using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stickman.Shooting
{
    public abstract class LongRangeWeapon<P, D> : Weapon where P : Projectile<D> where D : ProjectileData
    {
        public P ProjectilePrefab;

        public virtual P GetProjectile()
        {
            return Instantiate(ProjectilePrefab);
        }

        public abstract D GetProjectileData();

        public override void PerformShot()
        {
            var projectile = GetProjectile();
            var data = GetProjectileData();
            projectile.PerformShot(data);
        }
    }
}
