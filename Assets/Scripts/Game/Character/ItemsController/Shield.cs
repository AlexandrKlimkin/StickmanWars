using Character.Shooting;

namespace Game.Character.ItemsController {
    public class Shield : Weapon {
        public override ItemType ItemType => ItemType.Vehicle;
        public override WeaponInputProcessor InputProcessor =>
            _AutoFireProcessor ?? (_AutoFireProcessor = new AutoFireProcessor(this));

        private AutoFireProcessor _AutoFireProcessor;
    }
}