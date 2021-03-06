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
        public float HitForce;
        public float RecoilForce;
        public bool MagazineLimited = true;
        public int Magazine;

        public int ProjectilesInShot = 1;

        public float RateOfFire;
        public float ReloadTime;
        public float DispersionAngle;

        public float MinThrowStartSpeed;
        public float MaxThrowStartSpeed;
        public float MaxForceTime;
    }
}