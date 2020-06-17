using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class LongRangeWeapon<P, D> : Weapon where P : Projectile<D> where D : ProjectileDataBase, new()
    {
        public override ItemType ItemType => ItemType.Weapon;
        public string ProjectileName;

        public virtual P GetProjectile()
        {
            //return Instantiate(ProjectilePrefab);
            return VisualEffect.GetEffect<P>(ProjectileName);
        }

        public virtual D GetProjectileData()
        {
            var data = new D
            {
                Damage = GetDamage()
            };
            return data;
        }

        public override void PerformShot()
        {
            for (int i = 0; i < Stats.ProjectilesInShot; i++) {
                var projectile = GetProjectile();
                var data = GetProjectileData();
                projectile.Setup(data);
                projectile.Play();
                if (!PickableItem.Owner.MovementController.LedgeHang)
                    AddRecoil(data.Rotation * -Vector3.forward);
            }
        }

        private void AddRecoil(Vector2 direction) {
            direction.y *= 0.3f;
            PickableItem.Owner.Rigidbody2D.AddForce(direction * Stats.RecoilForce);
        }
    }
}