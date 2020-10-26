using UnityEngine;

namespace Character.Shooting {
    public class AutomateWeapon : MonoBehaviour {
        private Weapon _Weapon;

        private void Awake() {
            _Weapon = GetComponent<Weapon>();
            _Weapon.SetInputProcessor(new AutoFireProcessor(_Weapon));
        }
    }
}