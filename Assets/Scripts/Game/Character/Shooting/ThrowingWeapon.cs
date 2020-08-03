using Assets.Scripts.Tools;
using Character.Shooting.Character.Shooting;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

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
            data.Rotation = transform.rotation;
            data.StartVelocity = Mathf.Lerp(_Stats.MinThrowStartSpeed, _Stats.MaxThrowStartSpeed,  _FireForceProcessor.NormilizedForce);
            return data;
        }

        public override ThrowingProjectile GetProjectile() {
            return GetComponent<ThrowingProjectile>();
        }

        public override void PerformShot() {
            base.PerformShot();
            var projectile = GetProjectile();
            var data = GetProjectileData();
            projectile.Setup(data);
            PickableItem.Owner.WeaponController.ThrowOutMainWeapon(data.StartVelocity, -720f);
            projectile.Play();
            PickableItem.CanPickUp = false;
        }
    }
}
