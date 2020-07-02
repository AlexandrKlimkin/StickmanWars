using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Items;
using UnityEngine;

namespace Character.Shooting {
    public abstract class Weapon : MonoBehaviour {
        public abstract ItemType ItemType { get; }
        public virtual WeaponReactionType WeaponReaction => WeaponReactionType.Fire;

        public WeaponView WeaponView { get; protected set; }
        public PickableItem PickableItem { get; protected set; }

        public abstract WeaponInputProcessor InputProcessor { get; }

        [SerializeField]
        protected WeaponConfig _Stats;
        public WeaponPickupType PickupType;
        [SerializeField]
        private string _Id;
        public string Id => _Id;

        public WeaponConfig Stats => _Stats;
        public abstract void PerformShot();

        protected virtual bool UseThrowForce => true;

        protected virtual void Awake() {
            PickableItem = GetComponent<PickableItem>();
        }

        protected virtual void Start() {
            WeaponView = PickableItem.ItemView as WeaponView;
            WeaponsInfoContainer.AddWeapon(this);
        }

        public virtual void PickUp(CharacterUnit owner) {
            PickableItem.PickUp(owner);
            if (WeaponReaction == WeaponReactionType.Fire)
                owner?.WeaponController?.SubscribeWeaponOnEvents(this);
            else if (WeaponReaction == WeaponReactionType.Jump)
                owner?.MovementController?.SubscribeWeaponOnEvents(this);
        }

        public virtual void ThrowOut(CharacterUnit owner) {
            if (WeaponReaction == WeaponReactionType.Fire)
                owner?.WeaponController?.UnSubscribeWeaponOnEvents(this);
            else if (WeaponReaction == WeaponReactionType.Jump)
                owner?.MovementController?.UnSubscribeWeaponOnEvents(this);
            PickableItem.ThrowOut(WeaponView.ShootTransform.forward * Stats.MaxThrowForce);
        }

        protected virtual Damage GetDamage() {
            return new Damage(PickableItem.Owner.OwnerId, null, _Stats.Damage);
        }

        protected virtual void Enable() {
            WeaponsInfoContainer.AddWeapon(this);
        }

        protected virtual void OnDisable() {
            WeaponsInfoContainer.RemoveWeapon(this);
        }
    }

    public enum ItemType {
        Weapon,
        Vehicle
    }

    public enum WeaponReactionType {
        Fire,
        Jump,
    }

    public enum WeaponPickupType {
        ArmNear,
        Neck,
    }
}