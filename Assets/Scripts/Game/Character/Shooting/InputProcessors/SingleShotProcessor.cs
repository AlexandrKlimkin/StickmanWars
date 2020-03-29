using InputSystem;
using UnityEngine;

namespace Character.Shooting {
    public class SingleShotProcessor : WeaponInputProcessor {

        public SingleShotProcessor(Weapon weapon) : base(weapon) { }

        public override void Process(InputKit inputKit) {
            if (Input.GetKeyDown(inputKit.Attack1)) {
                Weapon.PerformShot();
            }
        }

    }
}