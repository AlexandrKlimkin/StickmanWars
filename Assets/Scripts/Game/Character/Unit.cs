using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Control;
using Character.Health;
using Character.Movement;
using Character.Shooting;
using Rendering;
using UnityEngine;

public class Unit : MonoBehaviour, IDamageable, ICameraTarget {
    public PlayerController PlayerController { get; private set; }
    public MovementController MovementController { get; private set; }
    public WeaponController WeaponController { get; private set; }

    public static List<Unit> Units = new List<Unit>();

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public bool Dead { get; private set; }

    public event Action OnApplyDamage;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        MovementController = GetComponent<MovementController>();
        WeaponController = GetComponent<WeaponController>();
        Collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        MaxHealth = 100f;//Todo: Config
        Health = MaxHealth;
        GameCameraBehaviour.Instance.Targets.Add(this);
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
        GameCameraBehaviour.Instance?.Targets.Remove(this);
        Units.Remove(this);
    }
}
