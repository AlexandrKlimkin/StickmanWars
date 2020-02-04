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
            Debug.DrawLine(ShootTransform.position, ShootTransform.position + ShootTransform.forward * 10f, Color.green, 3f);
            data.Speed = Stats.ProjectileSpeed;
            data.Force = Stats.Force;
            return data;
        }
    }
}
