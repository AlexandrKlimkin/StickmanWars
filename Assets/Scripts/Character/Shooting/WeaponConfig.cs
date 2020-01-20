using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stickman.Shooting
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/WeaponConfig", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public float Range;
        public float ReloadingTime;
        public float ProjectileSpeed;
    }
}