﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting
{
    public class BulletWeapon : LongRangeWeapon<BulletProjectile, ProjectileData>
    {
        public override ProjectileData GetProjectileData()
        {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = WeaponView.ShootTransform.position;
            data.Rotation = WeaponView.ShootTransform.rotation;
            Debug.DrawLine(WeaponView.ShootTransform.position, WeaponView.ShootTransform.position + WeaponView.ShootTransform.forward * 10f, Color.green, 3f);
            data.Speed = Stats.ProjectileSpeed;
            data.Force = Stats.Force;
            return data;
        }
    }
}
