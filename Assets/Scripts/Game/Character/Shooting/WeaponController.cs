using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Shooting
{
    public class WeaponController : MonoBehaviour
    {
        public Transform NearArmTransform;
        public Transform NearArmShoulder;
        public Transform NearArmFist;
        public float ThrowOutForce;
        public Weapon Weapon;

        public bool HasWeapon => Weapon != null;

        public Unit Owner { get; private set; }

        public WeaponPicker WeaponPicker { get; private set; }

        private void Awake()
        {
            Owner = GetComponent<Unit>();
            WeaponPicker = GetComponentInChildren<WeaponPicker>();
        }

        private void Start()
        {
            Weapon?.PickUp(Owner);
        }

        public void SetWeaponedHandPosition(Vector2 position)
        {
            NearArmTransform.position = position;
        }

        public void Fire()
        {
            if(HasWeapon)
                Weapon.PerformShot();
        }

        public void ThrowOutWeapon()
        {
            if (HasWeapon) {
                Weapon.ThrowOut();
                Weapon = null;
            }
        }

        public void TryPickUpWeapon(Weapon weapon)
        {
            if(HasWeapon)
                return;
            Weapon = weapon;
            weapon.PickUp(Owner);
        }
    }
}
