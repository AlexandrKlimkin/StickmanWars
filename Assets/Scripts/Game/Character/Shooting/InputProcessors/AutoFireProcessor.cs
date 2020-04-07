using InputSystem;
using UnityEngine;

namespace Character.Shooting {
    public class AutoFireProcessor : WeaponInputProcessor {
        public AutoFireProcessor(BulletWeapon weapon) : base(weapon) {
            CurrentMagazine = weapon.Stats.Magazine;
        }

        public int CurrentMagazine { get; private set; }

        private float _ShotTimer;
        private float _ReloadTimer;

        private float TimeBetweenShots => 1 / Weapon.Stats.RateOfFire;

        public override void Process(InputKit inputKit) {
            if (Input.GetKey(inputKit.Attack1)) {
                if (CurrentMagazine > 0 && _ShotTimer >= TimeBetweenShots) {
                    Weapon.PerformShot();
                    CurrentMagazine--;
                    _ShotTimer = 0;
                }
            }
            _ShotTimer += Time.deltaTime;
        }
    }
}
