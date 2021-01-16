using UnityEngine;

namespace Character.Shooting {
    public class FireForceProcessor : WeaponInputProcessor {

        public FireForceProcessor(Weapon weapon) : base(weapon) { }

        private float _Timer;

        public float NormilizedForce => Mathf.Clamp01(_Timer / Weapon.Stats.MaxForceTime);

        public override void ProcessHold() {
            _Timer += Time.deltaTime;
            base.ProcessHold();
        }

        public override void ProcessRelease() {
            CurrentMagazine--;
            Weapon.PerformShot();
            _Timer = 0;
            base.ProcessRelease();
        }
    }
}