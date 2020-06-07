using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class SimpleDamageable : MonoBehaviour, IDamageable
{
    public Collider2D Collider { get; set; }

    public event Action<SimpleDamageable, Damage> OnDamage;

    public void ApplyDamage(Damage damage)
    {
        OnDamage?.Invoke(this, damage);
    }

    private void Awake()
    {
        Collider = gameObject.GetComponentInChildren<Collider2D>();
    }
}
