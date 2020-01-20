using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stickman.Shooting
{
    public class BulletWeapon : LongRangeWeapon<BulletProjectile, ProjectileData>
    {
        public Transform ShootTransform;

        public override ProjectileData GetProjectileData()
        {
            return new ProjectileData
            {
                BirthTime = Time.time,
                LifeTime = Stats.Range / Stats.ProjectileSpeed,
                Position = ShootTransform.position,
                Rotation = ShootTransform.rotation,
                Speed = Stats.ProjectileSpeed,
            };
        }
    }
}
