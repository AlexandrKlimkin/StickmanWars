using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

public class SimpleDamageable : MonoBehaviour, IDamageable {
    [Dependency]
    private readonly HealthService _HealthService;
    [Dependency]
    private readonly SignalBus _SignalBus;

    public float StartHealth;
    public bool UseHealth = false;

    public Collider2D Collider { get; set; }

    public float Health { get; set; }

    public float NormilizedHealth => Health / MaxHealth;

    public byte? OwnerId => null;

    public float MaxHealth { get; set; }

    public bool Dead { get; set; }

    public event Action<SimpleDamageable, Damage> OnDamage;
    public event Action<SimpleDamageable, Damage> OnKill;

    public void ApplyDamage(Damage damage) {
        if(UseHealth)
            _SignalBus.FireSignal(new ApplyDamageSignal(damage));
        OnDamage?.Invoke(this, damage);
    }

    public void Kill(Damage damage) {
        OnKill?.Invoke(this, damage);
        Destroy(gameObject);
    }

    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        Collider = gameObject.GetComponentInChildren<Collider2D>();
        if (UseHealth) {
            MaxHealth = StartHealth;
            Health = MaxHealth;
        }
    }

}
