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
        public Transform NearArmFist;
        public Weapon Weapon;

        public bool HasWeapon => Weapon != null;

        public CharacterUnit Owner { get; private set; }

        public WeaponPicker WeaponPicker { get; private set; }

        private void Awake()
        {
            Owner = GetComponent<CharacterUnit>();
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

        public void Process(InputKit input)
        {
            Weapon?.InputProcessor.Process(input);
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
