using InputSystem;
using UnityEngine;

namespace Character.Shooting {

    namespace Character.Shooting {
        public class FireForceProcessor : WeaponInputProcessor {

            public FireForceProcessor(Weapon weapon) : base(weapon) { }

            private float _Timer;

            public float NormilizedForce => Mathf.Clamp01(_Timer / Weapon.Stats.MaxForceTime);

            public override void Process(InputKit inputKit) {
                if (Input.GetKey(inputKit.Attack1)) {
                    _Timer += Time.deltaTime;
                }
                if (Input.GetKeyUp(inputKit.Attack1)) {
                    Weapon.PerformShot();
                    _Timer = 0;
                }
            }
        }
    }
}