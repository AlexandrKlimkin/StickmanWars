using InputSystem;
using UnityEngine;

namespace Character.Shooting {
    public class AutoFireProcessor : WeaponInputProcessor {
        public AutoFireProcessor(Weapon weapon) : base(weapon) { }

        private float _ShotTimer;
        private float _ReloadTimer;

        private float TimeBetweenShots => 1 / Weapon.Stats.RateOfFire;

        public override void Process(InputKit inputKit) {
            if (Input.GetKey(inputKit.Attack1)) {
                if (_ShotTimer >= TimeBetweenShots) {
                    Weapon.PerformShot();
                    _ShotTimer = 0;
                }
                else {
                    _ShotTimer += Time.deltaTime;
                }
            }
        }
    }
}
