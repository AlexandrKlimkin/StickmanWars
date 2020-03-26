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
        public List<Weapon> EquipedWeapons = new List<Weapon>();

        public bool HasWeapon => EquipedWeapons.Count > 0;

        public Unit Owner { get; private set; }

        public WeaponPicker WeaponPicker { get; private set; }

        private void Awake()
        {
            Owner = GetComponent<Unit>();
            WeaponPicker = GetComponentInChildren<WeaponPicker>();
        }

        private void Start()
        {
            EquipedWeapons.ForEach(_=>_.PickUp(Owner));
        }

        public void SetWeaponedHandPosition(Vector2 position)
        {
            NearArmTransform.position = position;
        }

        public void Fire()
        {
            for(int i = 0; i < EquipedWeapons.Count; i++) {
                var weapon = EquipedWeapons[i];
                weapon.PerformShot();
            }
        }

        public void ThrowOutWeapon()
        {
            EquipedWeapons.ForEach(_ => _.ThrowOut());
            EquipedWeapons.Clear();
        }

        public void TryPickUpWeapon(Weapon weapon)
        {
            if(EquipedWeapons.Count > 0)
                return;
            EquipedWeapons.Add(weapon);
            weapon.PickUp(Owner);
        }
    }
}
