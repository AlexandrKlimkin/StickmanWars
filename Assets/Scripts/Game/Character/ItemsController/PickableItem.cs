using Character.Shooting;
using UnityEngine;

namespace Items {
    public class PickableItem : MonoBehaviour {
        [SerializeField]
        protected WeaponPickupType _PickupType;

        public CharacterUnit Owner { get; protected set; }
        [SerializeField] private bool _canPickUp = true;
        public ItemView ItemView { get; protected set; }
        protected virtual void Awake() {
            ItemView = GetComponent<ItemView>();
        }

        public virtual void ThrowOut(Vector2? startVelocity, float? angularVel) {
            ItemView.ThrowOut(Owner.WeaponController.gameObject);
            if(startVelocity != null)
                ItemView.Rigidbody.velocity = startVelocity.Value;
            if(angularVel != null)
                ItemView.Rigidbody.angularVelocity = angularVel.Value;
            ItemView.Levitation.DisableOnTime(6f);
            Owner = null;
        }

        public virtual void PickUp(CharacterUnit pickuper) {
            if (_canPickUp)
            {
                Owner = pickuper;
                var target = GetPickupTransform(_PickupType);
                ItemView.PickUp(target);
            }
            
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