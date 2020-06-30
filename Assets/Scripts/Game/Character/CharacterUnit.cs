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
using Core.Services.Game;

public class CharacterUnit : MonoBehaviour, IDamageable, ICameraTarget {
    [Dependency]
    private readonly SignalBus _SignalBus;

    public MovementController MovementController { get; private set; }
    public WeaponController WeaponController { get; private set; }

    public static List<CharacterUnit> Characters = new List<CharacterUnit>();

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public float NormilizedHealth => Health / MaxHealth;

    public bool Dead { get; private set; }

    public byte OwnerId { get; private set; }
    public string CharacterId;

    public event Action<Damage> OnApplyDamage;
    public event Action<Damage> OnApplyDeathDamage;

    private void Awake() {
        MovementController = GetComponent<MovementController>();
        WeaponController = GetComponent<WeaponController>();
        Collider = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Characters.Add(this);
    }

    public Collider2D Collider { get; set; }
    public Rigidbody2D Rigidbody2D { get; set; }

    public Vector3 Position => transform.position;
    public Vector3 Velocity => MovementController.Velocity;
    public float Direction => MovementController.Direction;

    byte? IDamageable.OwnerId => OwnerId;

    public void ApplyDamage(Damage damage) {
        if (Dead)
            return;
        if (damage.InstigatorId == OwnerId) //ToDo: friendly fire, game config
            return;
        Health -= damage.Amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        OnApplyDamage?.Invoke(damage);
        if (Health <= 0)
            OnApplyDeathDamage?.Invoke(damage);
    }

    public void Initialize(byte ownerId, string characterId) {
        ContainerHolder.Container.BuildUp(this);
        OwnerId = ownerId;
        CharacterId = characterId;
        MaxHealth = 100f; //Todo: Config
        Health = MaxHealth;
    }

    private void OnDestroy() {
        _SignalBus?.FireSignal(new GameCameraTargetsChangeSignal(this, false));
        _SignalBus?.UnSubscribeFromAll(this);
        Characters.Remove(this);
    }

    public void Kill() {
        Dead = true;
        Debug.Log($"Player {OwnerId} character {CharacterId} dead.");
        Destroy(gameObject); //ToDo: something different
    }
}
