using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stickman.Shooting
{
    public class WeaponController : MonoBehaviour
    {
        public Transform NearArmTransform;
        public List<Weapon> Weapons;

        private Camera _Camera;

        private void Start()
        {
            _Camera = Camera.main;
        }

        private void LateUpdate()
        {
            NearArmTransform.position = _Camera.ScreenToWorldPoint(Input.mousePosition);
        }

        public void Fire()
        {
            Weapons.ForEach(_=>_.PerformShot());
        }
    }
}
