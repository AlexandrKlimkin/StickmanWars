using System;
using UnityEngine;

namespace Character.Shooting {
    public abstract class WeaponInputProcessor {

        public event Action<int> OnProcessHold;
        public event Action<int> OnProcessRelease;
        public event Action<int> OnProcessPress;


        public int CurrentMagazine { get; protected set; }
        protected Weapon Weapon { get; private set; }
        public WeaponInputProcessor(Weapon weapon) {
            Weapon = weapon;
        }

        public void SetMagazine(int ammo) {
            CurrentMagazine = ammo;
        }

        public virtual void ProcessHold() {
            OnProcessHold?.Invoke(CurrentMagazine);
        }
        public virtual void ProcessRelease() {
            OnProcessRelease?.Invoke(CurrentMagazine);
        }
        public virtual void ProcessPress() {
            OnProcessPress?.Invoke(CurrentMagazine);
        }
        public virtual void Process() { }
    }
}