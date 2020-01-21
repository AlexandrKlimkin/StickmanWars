using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting
{
    public class WeaponController : MonoBehaviour
    {
        public Transform NearArmTransform;
        public List<Weapon> Weapons;

        public Unit Owner { get; private set; }

        private void Awake()
        {
            Owner = GetComponent<Unit>();
        }

        private void Start()
        {
            Weapons.ForEach(_=>_.PickUp(Owner));
        }

        public void SetWeaponedHandPosition(Vector2 position)
        {
            NearArmTransform.position = position;
        }

        public void Fire()
        {
            Weapons.ForEach(_=>_.PerformShot());
        }
    }
}
