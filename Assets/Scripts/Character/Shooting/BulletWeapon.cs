using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting
{
    public class BulletWeapon : LongRangeWeapon<BulletProjectile, ProjectileData>
    {
        public Transform ShootTransform;

        public override ProjectileData GetProjectileData()
        {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = ShootTransform.position;
            data.Rotation = ShootTransform.rotation;
            data.Speed = Stats.ProjectileSpeed;
            return data;
        }
    }
}
