using Assets.Scripts.Tools;
using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting {
    public abstract class LongRangeWeapon<P, D> : Weapon where P : Projectile<D> where D : ProjectileDataBase, new() {

        [SerializeField]
        private Transform _ShellEffectPoint;
        [SerializeField]
        private string _ShellEffectName;
        [SerializeField]
        private List<Transform> _MuzzleFlashEffectPoints;
        [SerializeField]
        private List<string> _MuzzleFlashEffectNames;

        public override ItemType ItemType => ItemType.Weapon;
        public string ProjectileName;

        protected float RandomDispersionAngle => Random.Range(-_Stats.DispersionAngle / 2, _Stats.DispersionAngle / 2);

        public virtual P GetProjectile() {
            //return Instantiate(ProjectilePrefab);
            return VisualEffect.GetEffect<P>(ProjectileName);
        }

        public virtual D GetProjectileData() {
            var data = new D {
                Damage = GetDamage()
            };
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
            data.OwnerId = PickableItem.Owner.OwnerId;
            Debug.DrawLine(WeaponView.ShootTransform.position, WeaponView.ShootTransform.position + WeaponView.ShootTransform.forward * 10f, Color.green, 3f);
            return data;
        }

        public override void PerformShot() {
            base.PerformShot();
            for (int i = 0; i < Stats.ProjectilesInShot; i++) {
                var projectile = GetProjectile();
                var data = GetProjectileData();
                projectile.Setup(data);
                projectile.Play();
                if (!PickableItem.Owner.MovementController.LedgeHang)
                    AddRecoil(data.Rotation * -Vector3.forward);
            }
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

        private void AddRecoil(Vector2 direction) {
            direction.y *= 0.6f;
            PickableItem.Owner.Rigidbody2D.AddForce(direction * Stats.RecoilForce);
        }


    }
}