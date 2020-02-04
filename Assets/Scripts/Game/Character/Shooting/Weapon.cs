using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

namespace Character.Shooting
{
    public abstract class Weapon : MonoBehaviour
    {
        public Unit Owner { get; protected set; }

        [SerializeField] private WeaponConfig _Stats;
        public WeaponConfig Stats => _Stats;
        public abstract void PerformShot();

        protected virtual Damage GetDamage()
        {
            return new Damage(Owner, _Stats.Damage);
        }

        public virtual void PickUp(Unit pickuper) {
            Owner = pickuper;
        }
    }
}