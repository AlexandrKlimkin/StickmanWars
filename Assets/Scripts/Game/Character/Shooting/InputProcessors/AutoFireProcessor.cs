using UnityEngine;

namespace Character.Shooting {
    public class AutoFireProcessor : RateOfFireProcessor {
        public AutoFireProcessor(Weapon weapon) : base(weapon) { }

        public override void ProcessHold()
        {
            TryToShot();
            base.ProcessHold();
        }
    }
}
