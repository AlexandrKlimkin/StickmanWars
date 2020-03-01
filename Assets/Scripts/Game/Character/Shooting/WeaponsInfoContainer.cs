using System.Collections;
using System.Collections.Generic;
using Character.Shooting;
using UnityEngine;

public static class WeaponsInfoContainer
{
    public static List<Weapon> AllWeapons = new List<Weapon>();

    public static void AddWeapon(Weapon weapon) {
        if(!AllWeapons.Contains(weapon))
            AllWeapons.Add(weapon);
    }

    public static void RemoveWeapon(Weapon weapon)
    {
        if(AllWeapons.Contains(weapon))
            AllWeapons.Remove(weapon);
    }
}
