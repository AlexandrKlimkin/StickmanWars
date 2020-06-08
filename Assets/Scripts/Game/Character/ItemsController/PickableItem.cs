﻿using Character.Shooting;
using UnityEngine;

namespace Items {
    public class PickableItem : MonoBehaviour {
        [SerializeField]
        protected WeaponPickupType _PickupType;

        public CharacterUnit Owner { get; protected set; }

        public ItemView ItemView { get; protected set; }

        protected virtual void Awake() {
            ItemView = GetComponent<ItemView>();
        }

        public virtual void ThrowOut(Vector2 force) {
            ItemView.ThrowOut(Owner.WeaponController.gameObject);
            ItemView.Rigidbody.AddForce(force);
            ItemView.Rigidbody.angularVelocity = -360f;
            ItemView.Levitation.DisableOnTime(6f);
            Owner = null;
        }

        public virtual void PickUp(CharacterUnit pickuper) {
            Owner = pickuper;
            var target = GetPickupTransform(_PickupType);
            ItemView.PickUp(target);
        }


        private Transform GetPickupTransform(WeaponPickupType pickupType)
        {
            switch (pickupType)
            {
                case WeaponPickupType.ArmNear:
                    return Owner.WeaponController.NearArmWeaponTransform;
                case WeaponPickupType.Neck:
                    return Owner.WeaponController.NeckWeaponTransform;
                default:
                    return null;
            }
        }

    }
}