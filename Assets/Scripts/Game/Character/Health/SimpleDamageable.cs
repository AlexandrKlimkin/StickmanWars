using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class SimpleDamageable : MonoBehaviour, IDamageable
{
    public Collider2D Collider { get; set; }

    public float Health => 0;

    public float NormilizedHealth => 1;

    public byte? OwnerId => null;

    public event Action<SimpleDamageable, Damage> OnDamage;

    public void ApplyDamage(Damage damage)
    {
        OnDamage?.Invoke(this, damage);
    }

    public void Kill() {
        throw new NotImplementedException();
    }

    private void Awake()
    {
        Collider = gameObject.GetComponentInChildren<Collider2D>();
    }
}
