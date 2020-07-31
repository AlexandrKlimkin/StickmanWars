using RC.UI.Markers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting {
    [RequireComponent(typeof(Weapon))]
    public class FallDownIfNoAmmo : MonoBehaviour {

        private Weapon _Weapon;

        private void Awake() {
            _Weapon = GetComponent<Weapon>();
            _Weapon.OnNoAmmo += OnNoAmmo;
        }

        private void OnNoAmmo() {
            StartCoroutine(MakeFallingDownRoutine());
        }

        private IEnumerator MakeFallingDownRoutine() {
            yield return null;
            if (_Weapon.PickableItem.Owner != null) {
                if (_Weapon.ItemType == ItemType.Weapon) {
                    _Weapon.PickableItem.Owner.WeaponController.ThrowOutMainWeapon(_Weapon.PickableItem.Owner.Rigidbody2D.velocity / 2f, Random.Range(-180, 540f));
                } else
                if (_Weapon.ItemType == ItemType.Vehicle) {
                    _Weapon.PickableItem.Owner.WeaponController.ThrowOutVehicle(_Weapon.PickableItem.Owner.Rigidbody2D.velocity / 3f, Random.Range(-180, 540f));
                }
            }
            var itemProvider = _Weapon.gameObject.GetComponent<ItemMarkerProvider>();
            if (itemProvider != null)
                Destroy(itemProvider);
            _Weapon.WeaponView.MakeFallingDown();
        }
    }
}