using InputSystem;
using UnityEngine;

namespace Character.Shooting {
    public abstract class WeaponInputProcessor
    {
        protected Weapon Weapon { get; private set; }

        public WeaponInputProcessor(Weapon weapon)
        {
            Weapon = weapon;
        }

        public virtual void ProcessHold() { }
        public virtual void ProcessRelease() { }
        public virtual void ProcessPress() { }
        public virtual void Process() { }
    }
}