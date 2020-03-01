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
        public List<Weapon> EquipedWeapons;

        public bool HasWeapon => EquipedWeapons.Count > 0;

        public Unit Owner { get; private set; }

        private void Awake()
        {
            Owner = GetComponent<Unit>();
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
            EquipedWeapons.ForEach(_=>_.PerformShot());
        }

        public void ThrowOutWeapon()
        {
            //var first = EquipedWeapons.FirstOrDefault();
            //if(first == null)
            //    return;
            //first.ThrowOut(Owner);
            //var f = first.WeaponView.ShootTransform.forward;
            //var vector = new Vector2(f.z, f.y);
            //first.WeaponView.Rigidbody.AddForce(f * ThrowOutForce);
            //EquipedWeapons.Remove(first);
        }

        public void TryPickUpWeapon(Weapon weapon)
        {
            if(EquipedWeapons.Count > 0)
                return;
            EquipedWeapons = new List<Weapon>{ weapon };
            weapon.PickUp(Owner);
        }
    }
}
