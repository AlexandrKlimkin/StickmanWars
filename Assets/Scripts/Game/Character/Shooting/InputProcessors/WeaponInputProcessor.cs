using InputSystem;

namespace Character.Shooting {
    public abstract class WeaponInputProcessor
    {
        protected Weapon Weapon { get; private set; }

        public WeaponInputProcessor(Weapon weapon)
        {
            Weapon = weapon;
        }

        public abstract void Process(InputKit inputKit);
    }
}