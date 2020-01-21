using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class LongRangeWeapon<P, D> : Weapon where P : Projectile<D> where D : ProjectileData, new()
    {
        public P ProjectilePrefab;

        public virtual P GetProjectile()
        {
            return Instantiate(ProjectilePrefab);
        }

        public virtual D GetProjectileData()
        {
            var data = new D
            {
                Damage = GetDamage()
            };
            return data;
        }

        public override void PerformShot()
        {
            var projectile = GetProjectile();
            var data = GetProjectileData();
            projectile.PerformShot(data);
        }
    }
}
