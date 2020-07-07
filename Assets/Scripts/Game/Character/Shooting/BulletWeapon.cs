using Assets.Scripts.Tools;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class BulletWeapon : LongRangeWeapon<BulletProjectile, BulletProjectileData> {

        [SerializeField]
        private Transform _ShellEffectPoint;
        [SerializeField]
        private string _ShellEffectName;
        [SerializeField]
        private List<Transform> _MuzzleFlashEffectPoints;
        [SerializeField]
        private List<string> _MuzzleFlashEffectNames;

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
            if (_ShellEffectPoint != null && !string.IsNullOrEmpty(_ShellEffectName)) {
                var effect = VisualEffect.GetEffect<ParticleEffect>(_ShellEffectName);
                effect.transform.position = _ShellEffectPoint.position;
                effect.transform.rotation = _ShellEffectPoint.rotation;
                effect.Play();
            }
            if (_MuzzleFlashEffectPoints != null && _MuzzleFlashEffectPoints.Count > 0 && _MuzzleFlashEffectNames != null && _MuzzleFlashEffectNames.Count > 0) {
                var randIndex = Random.Range(0, _MuzzleFlashEffectNames.Count);
                var randPointIndex = Random.Range(0, _MuzzleFlashEffectPoints.Count);
                var effect = VisualEffect.GetEffect<AttachedParticleEffect>(_MuzzleFlashEffectNames[randIndex]);
                //effect.transform.position = _ShellEffectPoint.position;
                //effect.transform.rotation = _ShellEffectPoint.rotation;
                effect.SetTarget(_MuzzleFlashEffectPoints[randPointIndex]);
                effect.Play();
            }
        }

    }
}
