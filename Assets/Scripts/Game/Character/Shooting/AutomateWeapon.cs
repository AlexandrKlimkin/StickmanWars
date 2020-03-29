namespace Character.Shooting {
    public class AutomateWeapon : BulletWeapon {
        public override WeaponInputProcessor InputProcessor => _AutoFireProcessor ?? (_AutoFireProcessor = new AutoFireProcessor(this));
        private AutoFireProcessor _AutoFireProcessor;
    }
}