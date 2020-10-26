﻿using Assets.Scripts.Tools;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting {
    public class BulletWeapon : LongRangeWeapon<BulletProjectile, BulletProjectileData> {

        public override BulletProjectileData GetProjectileData()
        {
            var data = base.GetProjectileData();
            data.Speed = Stats.ProjectileSpeed;
            data.Force = Stats.HitForce;
            return data;
        }

    }
}