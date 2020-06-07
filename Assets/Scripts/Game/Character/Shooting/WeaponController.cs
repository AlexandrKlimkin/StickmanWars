using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InputSystem;
using UnityEngine;

namespace Character.Shooting
{
    public class WeaponController : MonoBehaviour
    {
        public Transform NearArmTransform;
        public Transform NearArmShoulder;
        public Transform NearArmWeaponTransform;
        public Transform NeckWeaponTransform;
        public Weapon MainWeapon;
        public Weapon Vehicle;

        public bool HasMainWeapon => MainWeapon != null;
        public bool HasVehicle => Vehicle != null;

        public event Action OnPressFire;
        public event Action OnHoldFire;
        public event Action OnReleaseFire;

        public CharacterUnit Owner { get; private set; }
        public WeaponPicker WeaponPicker { get; private set; }

        private void Awake()
        {
            Owner = GetComponent<CharacterUnit>();
            WeaponPicker = GetComponentInChildren<WeaponPicker>();
        }

        private void Start()
        {
            MainWeapon?.PickableItem.PickUp(Owner);
            Vehicle?.PickableItem.PickUp(Owner);
        }

        private void Update()
        {
            MainWeapon?.InputProcessor.Process();
            Vehicle?.InputProcessor.Process();
        }


        public void SetWeaponedHandPosition(Vector2 position)
        {
            NearArmTransform.position = position;
        }

        public void HoldFire()
        {
            OnHoldFire?.Invoke();
        }

        public void PressFire()
        {
            OnPressFire?.Invoke();
        }

        public void ReleaseFire()
        {
            OnReleaseFire?.Invoke();
        }

        public void ThrowOutMainWeapon()
        {
            if (!HasMainWeapon) return;
            MainWeapon.ThrowOut(Owner);
            MainWeapon = null;
        }

        public void ThrowOutVehicle()
        {
            if (!HasVehicle) return;
            Vehicle.ThrowOut(Owner);
            Vehicle = null;
        }

        public void TryPickUpWeapon(Weapon weapon)
        {
            if (weapon.ItemType == ItemType.Weapon)
            {
                if (HasMainWeapon)
                    return;
                MainWeapon = weapon;
                weapon.PickUp(Owner);
            }
            else if (weapon.ItemType == ItemType.Vehicle)
            {
                if (HasVehicle)
                    return;
                Vehicle = weapon;
                weapon.PickUp(Owner);
            }
        }

        public void SubscribeWeaponOnEvents(Weapon weapon)
        {
            OnHoldFire += weapon.InputProcessor.ProcessHold;
            OnPressFire += weapon.InputProcessor.ProcessPress;
            OnReleaseFire += weapon.InputProcessor.ProcessRelease;
        }

        public void UnSubscribeWeaponOnEvents(Weapon weapon)
        {
            OnHoldFire -= weapon.InputProcessor.ProcessHold;
            OnPressFire -= weapon.InputProcessor.ProcessPress;
            OnReleaseFire -= weapon.InputProcessor.ProcessRelease;
        }
    }
}