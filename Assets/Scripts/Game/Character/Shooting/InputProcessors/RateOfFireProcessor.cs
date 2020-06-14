using UnityEngine;

namespace Character.Shooting
{
    public abstract class RateOfFireProcessor : WeaponInputProcessor
    {
        protected float _ShotTimer;
        protected float _ReloadTimer;
        protected float TimeBetweenShots => 1 / Weapon.Stats.RateOfFire;

        protected RateOfFireProcessor(Weapon weapon) : base(weapon)
        {
            CurrentMagazine = weapon.Stats.Magazine;
            _ShotTimer = TimeBetweenShots;
        }

        public override void Process() {
            _ShotTimer += Time.deltaTime;
        }

        protected void TryToShot()
        {
            if (CurrentMagazine <= 0 || _ShotTimer < TimeBetweenShots) return;
            Weapon.PerformShot();
            CurrentMagazine--;
            _ShotTimer = 0;
        }
    }
}