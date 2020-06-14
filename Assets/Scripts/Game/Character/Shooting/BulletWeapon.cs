﻿using Assets.Scripts.Tools;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class BulletWeapon : LongRangeWeapon<BulletProjectile, BulletProjectileData> {

        private float RandomDispersionAngle => Random.Range(-_Stats.DispersionAngle / 2, _Stats.DispersionAngle / 2);

        [SerializeField]
        private Transform _ShellEffectPoint;
        [SerializeField]
        private string _ShellEffectName;

        public override BulletProjectileData GetProjectileData()
        {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = WeaponView.ShootTransform.position;

            Vector3 shootRotEuler;
            var directionVector = PickableItem.Owner.WeaponController.AimPosition - WeaponView.ShootTransform.position.ToVector2();
            shootRotEuler = Quaternion.LookRotation(directionVector).eulerAngles;
            if (_Stats.DispersionAngle != 0) {
                shootRotEuler = new Vector3(shootRotEuler.x + RandomDispersionAngle, shootRotEuler.y, shootRotEuler.z);
            }
            data.Rotation = Quaternion.Euler(shootRotEuler);

           Debug.DrawLine(WeaponView.ShootTransform.position, WeaponView.ShootTransform.position + WeaponView.ShootTransform.forward * 10f, Color.green, 3f);
            data.Speed = Stats.ProjectileSpeed;
            data.Force = Stats.HitForce;
            return data;
        }

        public override void PerformShot() {
            base.PerformShot();
            if (_ShellEffectPoint == null)
                return;
            if(string.IsNullOrEmpty(_ShellEffectName))
                return;
            var effect = VisualEffect.GetEffect<ParticleEffect>(_ShellEffectName);
            effect.transform.position = _ShellEffectPoint.position;
            effect.transform.rotation = _ShellEffectPoint.rotation;
            effect.Play();
        }

    }
}
