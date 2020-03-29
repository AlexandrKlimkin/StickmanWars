namespace Character.Shooting {
    public class PistolWeapon : BulletWeapon {
        public override WeaponInputProcessor InputProcessor => _SingleShotProcessor ?? (_SingleShotProcessor = new SingleShotProcessor(this));
        private SingleShotProcessor _SingleShotProcessor;
    }
}