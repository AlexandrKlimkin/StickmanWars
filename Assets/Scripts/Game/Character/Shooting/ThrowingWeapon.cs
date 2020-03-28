using UnityEngine;

namespace Character.Shooting
{
    public class ThrowingWeapon : LongRangeWeapon<ThrowingProjectile, ThrowingProjectileData>
    {
        public bool CanBePicked { get; private set; }

        public override ThrowingProjectileData GetProjectileData()
        {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = transform.position;
            data.Rotation = transform.rotation;
            data.StartDirection = WeaponView.ShootTransform.forward;
            data.StartForce = _Stats.StartForce;
            return data;
        }

        public override ThrowingProjectile GetProjectile()
        {
            return GetComponent<ThrowingProjectile>();
        }

        public override void PerformShot()
        {
            var projectile = GetProjectile();
            var data = GetProjectileData();
            Owner.WeaponController.ThrowOutWeapon();
            projectile.Setup(data);
            projectile.Play();
        }
    }
}
