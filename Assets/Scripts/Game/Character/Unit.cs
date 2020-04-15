using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Control;
using Character.Health;
using Character.Movement;
using Character.Shooting;
using Game.CameraTools;
using KlimLib.SignalBus;
using UnityEngine;
using UnityDI;

public class Unit : MonoBehaviour, IDamageable, ICameraTarget {
    [Dependency]
    private readonly SignalBus _SignalBus;

    public MovementController MovementController { get; private set; }
    public WeaponController WeaponController { get; private set; }

    public static List<Unit> Units = new List<Unit>();

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public bool Dead { get; private set; }

    public event Action OnApplyDamage;

    private void Awake()
    {
        MovementController = GetComponent<MovementController>();
        WeaponController = GetComponent<WeaponController>();
        Collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ContainerHolder.Container.BuildUp(this); //Todo: Move to player spawn service
        MaxHealth = 100f;//Todo: Config
        Health = MaxHealth;
        _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, true)); //Todo: Move to player spawn service
        Units.Add(this);
    }

    public Collider2D Collider { get; set; }

    public Vector3 Position => transform.position;
    public Vector3 Velocity => MovementController.Velocity;
    public float Direction => MovementController.Direction;

    public void ApplyDamage(Damage damage)
    {
        if(Dead)
            return;
        if(damage.Instigator == this) //ToDo: friendly fire, game config
            return;
        Health -= damage.Amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        if(Health <=0)
            Kill();
        OnApplyDamage?.Invoke();
    }

    private void Kill()
    {
        Dead = true;
        Destroy(gameObject); //ToDo: something different
    }

    private void OnDestroy()
    {
        _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, false));
        _SignalBus.UnSubscribeFromAll(this);
        Units.Remove(this);
    }
}
