using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.Shooting {
    public class WeaponPicker : MonoBehaviour {
        private WeaponController _WeaponController;

        public Collider2D PickCollider { get; private set; }


        private void Awake() {
            PickCollider = GetComponent<Collider2D>();
            _WeaponController = GetComponentInParent<WeaponController>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var weapon = other.gameObject.GetComponentInParent<Weapon>();
            if (weapon != null) {
                _WeaponController.TryPickUpWeapon(weapon);
            }
        }
    }
}
