using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class LongRangeWeapon<P, D> : Weapon where P : Projectile<D> where D : ProjectileDataBase, new()
    {
        public string ProjectileName;

        public virtual P GetProjectile()
        {
            //return Instantiate(ProjectilePrefab);
            return VisualEffect.GetEffect<P>(ProjectileName);
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
            projectile.Setup(data);
            projectile.Play();
        }
    }
}