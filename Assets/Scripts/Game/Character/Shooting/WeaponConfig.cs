﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "EquipedWeapons/WeaponConfig", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public float Range;
        public float ReloadingTime;
        public float ProjectileSpeed;
        public float Damage;
        public float Force;
    }
}