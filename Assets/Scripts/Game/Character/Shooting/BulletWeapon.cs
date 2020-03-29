using UnityEngine;

namespace Character.Shooting
{
    public abstract class BulletWeapon : LongRangeWeapon<BulletProjectile, BulletProjectileData>
    {
        public override BulletProjectileData GetProjectileData()
        {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = WeaponView.ShootTransform.position;

            if (_Stats.DispersionAngle != 0) {
                var shootTransformRot = WeaponView.ShootTransform.rotation.eulerAngles;
                data.Rotation = Quaternion.Euler(shootTransformRot.x + RandomDispersionAngle, shootTransformRot.y, shootTransformRot.z);
            }
            else {
                data.Rotation = WeaponView.ShootTransform.rotation;
            }

            Debug.DrawLine(WeaponView.ShootTransform.position, WeaponView.ShootTransform.position + WeaponView.ShootTransform.forward * 10f, Color.green, 3f);
            data.Speed = Stats.ProjectileSpeed;
            data.Force = Stats.HitForce;
            return data;
        }

        private float RandomDispersionAngle => Random.Range(-_Stats.DispersionAngle / 2, _Stats.DispersionAngle / 2);
    }
}
