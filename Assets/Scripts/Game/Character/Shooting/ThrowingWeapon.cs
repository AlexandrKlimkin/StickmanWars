using Assets.Scripts.Tools;
using Character.Health;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Shooting {
    public class ThrowingWeapon : LongRangeWeapon<ThrowingProjectile, ThrowingProjectileData> {
        public override WeaponInputProcessor InputProcessor => _FireForceProcessor ?? (_FireForceProcessor = new FireForceProcessor(this));
        public bool CanBePicked { get; private set; }

        private FireForceProcessor _FireForceProcessor;
        protected override bool UseThrowForce => false;

        public override ThrowingProjectileData GetProjectileData() {
            var data = base.GetProjectileData();
            data.BirthTime = Time.time;
            data.LifeTime = Stats.Range / Stats.ProjectileSpeed;
            data.Position = transform.position;
            data.Rotation = transform.rotation;
            data.StartVelocity = Mathf.Lerp(_Stats.MinThrowStartSpeed, _Stats.MaxThrowStartSpeed,  _FireForceProcessor.NormilizedForce);

            if (Stats.TensionDamage && Stats.MinTensionDamage != 0 && Stats.MaxTensionDamage != 0)
            {
                int damage;
                damage = (int)Mathf.Lerp(Stats.MinTensionDamage, Stats.MaxTensionDamage, _FireForceProcessor.NormilizedForce);
                data.Damage.Amount = damage;
            }
            else if (Stats.TensionDamage)
            {
                Debug.LogError("You selected tension but did not specify maximum or minimum damage");
            }

            return data;
        }

        public override bool PickUp(CharacterUnit owner) {
            var pickedUp = base.PickUp(owner);
            InputProcessor.SetMagazine(1);
            return pickedUp;
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_Stats.CanPickedUp)
            {
                StartCoroutine(PickUpItem());
            }
        }

        IEnumerator PickUpItem()
        {
            yield return new WaitForSeconds(1);
            PickableItem.CanPickUp = true;
        }

    }
}
