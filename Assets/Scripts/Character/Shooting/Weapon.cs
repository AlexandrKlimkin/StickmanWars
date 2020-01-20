using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stickman.Shooting
{
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponConfig Stats => _Stats;
        public abstract void PerformShot();

        [SerializeField] private WeaponConfig _Stats;
    }
}