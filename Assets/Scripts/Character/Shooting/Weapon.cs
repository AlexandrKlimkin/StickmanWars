using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    WeaponConfig Stats { get; }
    void Fire();
}

public abstract class Weapon : MonoBehaviour, IWeapon
{
    public WeaponConfig Stats => _Stats;
    public abstract void Fire();

    [SerializeField]
    private WeaponConfig _Stats;
}