using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class SimpleDamageable : MonoBehaviour, IDamageable {
    public Collider2D Collider { get; set; }

    public float Health { get; set; }

    public float NormilizedHealth => 1;

    public byte? OwnerId => null;

    public float MaxHealth => float.MaxValue;

    public bool Dead { get; set; }

    public event Action<SimpleDamageable, Damage> OnDamage;

    public void ApplyDamage(Damage damage) {
        OnDamage?.Invoke(this, damage);
    }

    public void Kill(Damage damage) {
        Destroy(gameObject);
    }

    private void Awake() {
        Collider = gameObject.GetComponentInChildren<Collider2D>();
    }
}
