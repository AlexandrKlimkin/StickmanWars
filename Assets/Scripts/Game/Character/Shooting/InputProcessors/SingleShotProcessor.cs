using UnityEngine;

namespace Character.Shooting {
    public class SingleShotProcessor : RateOfFireProcessor {
        public SingleShotProcessor(Weapon weapon) : base(weapon) { }

        public override void ProcessPress() {
            TryToShot();
            base.ProcessPress();
        }
    }
}