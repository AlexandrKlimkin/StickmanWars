using Assets.Scripts.Tools;
using Character.Shooting.Character.Shooting;
using UnityEngine;

namespace Character.Shooting {
    public class ThrowingWeapon : LongRangeWeapon<ThrowingProjectile, ThrowingProjectileData> {
        public override WeaponInputProcessor InputProcessor => _FireForceProcessor ?? (_FireForceProcessor = new FireForceProcessor(this));
        private FireForceProcessor _FireForceProcessor;

        public bool CanBePicked { get; private set; }

        protected override bool UseThrowForce => false;

        public override ThrowingProjectileData GetProjectileData() {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = transform.position;

            Vector3 shootRotEuler;
            if (PickableItem.Owner != null) {
                var directionVector = PickableItem.Owner.WeaponController.AimPosition - WeaponView.ShootTransform.position.ToVector2().normalized;
                data.StartDirection = directionVector;

                data.StartForce = Mathf.Lerp(_Stats.MinThrowForce, _Stats.MaxThrowForce, _FireForceProcessor.NormilizedForce);
            }
            data.Rotation = transform.rotation;
            return data;
        }

        public override ThrowingProjectile GetProjectile() {
            return GetComponent<ThrowingProjectile>();
        }

        public override void PerformShot() {
            var projectile = GetProjectile();
            var data = GetProjectileData();
            PickableItem.Owner.WeaponController.ThrowOutMainWeapon(data.StartForce, -720f);
            projectile.Setup(data);
            projectile.Play();
        }
    }
}
