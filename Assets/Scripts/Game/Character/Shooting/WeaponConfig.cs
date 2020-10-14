using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "EquipedWeapons/WeaponConfig", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Float")]
        public float Range;
        public float ReloadingTime;
        public float ProjectileSpeed;
        public float Damage;
        public float HitForce;
        public float RecoilForce;

        public float RateOfFire;
        public float ReloadTime;
        public float DispersionAngle;

        public float MinThrowStartSpeed;
        public float MaxThrowStartSpeed;
        public float MaxForceTime;

        [Header("Int")]
        public int Magazine;
        public int ProjectilesInShot = 1;

        [Header ("Bools")]
        public bool MagazineLimited = true;

        [Header("CanPickedUpAfterThrown")]
        public bool CanPickedUp = false;

        [Header("Damage up per time")]
        public bool TensionDamage = false;

        [Header("Min/Max damage under tension")]
        public float MinTensionDamage;
        public float MaxTensionDamage;
    }
}