using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class Weapon : MonoBehaviour
    {
        public Unit Owner { get; protected set; }
        public WeaponView WeaponView { get; protected set; }

        public abstract WeaponInputProcessor InputProcessor { get; protected set; }

        [SerializeField] protected WeaponConfig _Stats;
        public WeaponConfig Stats => _Stats;
        public abstract void PerformShot();

        protected virtual void Awake()
        {
            WeaponView = GetComponent<WeaponView>();
        }

        protected virtual void Start();

        protected virtual Damage GetDamage()
        {
            return new Damage(Owner, _Stats.Damage);
        }

        public virtual void ThrowOut()
        {
            WeaponView.ThrowOut(Owner.WeaponController.gameObject);
            Owner = null;
        }

        public virtual void PickUp(Unit pickuper) {
            Owner = pickuper;
            WeaponView.PickUp(pickuper.WeaponController.NearArmFist);
        }

        protected virtual void Enable()
        {
            WeaponsInfoContainer.AddWeapon(this);
        }

        protected virtual void OnDisable() {
            WeaponsInfoContainer.RemoveWeapon(this);
        }
    }
}