using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting {
    public class FireForceWeapon : MonoBehaviour {
        private Weapon _Weapon;

        private void Awake() {
            _Weapon = GetComponent<Weapon>();
            _Weapon.SetInputProcessor(new FireForceProcessor(_Weapon));
        }
    }
}