using Assets.Scripts.Tools;
using UnityEngine;

namespace Character.Shooting {
    public class RocketWeapon : LongRangeWeapon<RocketProjectile, RocketProjectileData> {
        public override RocketProjectileData GetProjectileData() {
            var data = base.GetProjectileData();
            data.Speed = Stats.ProjectileSpeed;
            data.Force = Stats.HitForce;
            return data;
        }
    }
}