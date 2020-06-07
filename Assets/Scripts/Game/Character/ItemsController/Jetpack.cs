using Character.Shooting;
using UnityEngine;

namespace Items.Jetpack
{
    public class Jetpack : Weapon
    {
        public override ItemType ItemType => ItemType.Vehicle;
        public override WeaponReactionType WeaponReaction => WeaponReactionType.Jump;
        public override WeaponInputProcessor InputProcessor => _AutoFireProcessor ?? (_AutoFireProcessor = new AutoFireProcessor(this));
        private AutoFireProcessor _AutoFireProcessor;

        public override void PerformShot()
        {
            AddRecoil(Vector2.up);
        }

        private void AddRecoil(Vector2 direction) {
            PickableItem.Owner.Rigidbody2D.AddForce(direction * Stats.RecoilForce);
        }
    }
}