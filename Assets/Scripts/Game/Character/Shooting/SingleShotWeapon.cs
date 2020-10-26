﻿using UnityEngine;

namespace Character.Shooting {
    public class SingleShotWeapon : MonoBehaviour {
        private Weapon _Weapon;

        private void Awake() {
            _Weapon = GetComponent<Weapon>();
            _Weapon.SetInputProcessor(new SingleShotProcessor(_Weapon));
        }
    }
}