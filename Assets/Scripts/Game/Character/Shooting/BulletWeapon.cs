using UnityEngine;

namespace Character.Shooting
{
    public class BulletWeapon : LongRangeWeapon<BulletProjectile, BulletProjectileData>
    {
        public override WeaponInputProcessor InputProcessor => throw new System.NotImplementedException();

        protected override void Start()
        {
            base.Start();
            InputProcessor = new SingleShotProcessor();
        }

        public override BulletProjectileData GetProjectileData()
        {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = WeaponView.ShootTransform.position;
            data.Rotation = WeaponView.ShootTransform.rotation;
            Debug.DrawLine(WeaponView.ShootTransform.position, WeaponView.ShootTransform.position + WeaponView.ShootTransform.forward * 10f, Color.green, 3f);
            data.Speed = Stats.ProjectileSpeed;
            data.Force = Stats.HitForce;
            return data;
        }
    }
}
