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
            yield return new WaitForSeconds(0.1f);
            _Weapon.PickableItem.ThrowOut(null, Random.Range(-180f, -540f));
            var itemProvider = _Weapon.gameObject.GetComponent<ItemMarkerProvider>();
            if (itemProvider != null)
                Destroy(itemProvider);
            _Weapon.WeaponView.MakeFallingDown();
        }
    }
}