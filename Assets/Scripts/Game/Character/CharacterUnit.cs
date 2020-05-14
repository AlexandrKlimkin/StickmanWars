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

public class CharacterUnit : MonoBehaviour, IDamageable, ICameraTarget {
    [Dependency]
    private readonly SignalBus _SignalBus;

    public MovementController MovementController { get; private set; }
    public WeaponController WeaponController { get; private set; }

    public static List<CharacterUnit> Characters = new List<CharacterUnit>();

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public bool Dead { get; private set; }

    public byte OwnerId { get; private set; }
    public string CharacterId { get; private set; }

    public event Action OnApplyDamage;

    private void Awake() {
        MovementController = GetComponent<MovementController>();
        WeaponController = GetComponent<WeaponController>();
        Collider = GetComponent<Collider2D>();
    }

    private void Start() {
        MaxHealth = 100f; //Todo: Config
        Health = MaxHealth;
        Characters.Add(this);
    }

    public Collider2D Collider { get; set; }

    public Vector3 Position => transform.position;
    public Vector3 Velocity => MovementController.Velocity;
    public float Direction => MovementController.Direction;

    public void ApplyDamage(Damage damage) {
        if (Dead)
            return;
        if (damage.Instigator == this) //ToDo: friendly fire, game config
            return;
        Health -= damage.Amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        if (Health <= 0)
            Kill();
        OnApplyDamage?.Invoke();
    }

    public void Initialize(byte ownerId, string characterId) {
        OwnerId = ownerId;
        CharacterId = characterId;
    }

    private void Kill() {
        Dead = true;
        Debug.Log($"Player {OwnerId} character {CharacterId} dead.");
        _SignalBus.FireSignal(new CharacterDeathSignal(OwnerId, CharacterId)); //ToDo: Move to service
        Destroy(gameObject); //ToDo: something different
    }

    private void OnDestroy() {
        _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, false));
        _SignalBus.UnSubscribeFromAll(this);
        Characters.Remove(this);
    }
}
