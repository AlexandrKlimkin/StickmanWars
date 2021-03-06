﻿using System.Collections;
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
using System;
using Core.Audio;

public class CharacterUnit : MonoBehaviour, IDamageable, ICameraTarget {
    [Dependency]
    private readonly SignalBus _SignalBus;
    [Dependency]
    private readonly AudioService _AudioService;

    public MovementController MovementController { get; private set; }
    public WeaponController WeaponController { get; private set; }

    public static List<CharacterUnit> Characters = new List<CharacterUnit>();

    public float Health { get; set; }
    public float MaxHealth { get; private set; }
    public float NormilizedHealth => Health / MaxHealth;

    public bool Dead { get; set; }

    [SerializeField]
    private byte _OwnerId;
    public byte OwnerId { get; private set; }
    public string CharacterId;
    public bool IsBot;
    public List<string> HitAudioEffects;
    public List<string> DeathAudioEffects;
    public event Action<Damage> OnApplyDamage;

    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        MovementController = GetComponent<MovementController>();
        WeaponController = GetComponent<WeaponController>();
        Collider = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Characters.Add(this);
        OwnerId = _OwnerId;
        MaxHealth = 100f;
        Health = MaxHealth;
    }

    public Collider2D Collider { get; set; }
    public Rigidbody2D Rigidbody2D { get; set; }

    public Vector3 Position => transform.position;
    public Vector3 Velocity => MovementController.Velocity;
    public float Direction => MovementController.Direction;

    byte? IDamageable.OwnerId => OwnerId;

    public void ApplyDamage(Damage damage) {
        _SignalBus.FireSignal(new ApplyDamageSignal(damage));
        OnApplyDamage?.Invoke(damage);
        PlayeHitSound(HitAudioEffects);
    }

    private void PlayeHitSound(List<string> sounds) {
        if (sounds == null || sounds.Count == 0)
            return;
        var randIndex = UnityEngine.Random.Range(0, sounds.Count);
        _AudioService.PlaySound3D(sounds[randIndex], false, false, transform.position);
    }

    public void Initialize(byte ownerId, string characterId, bool isBot) {
        ContainerHolder.Container.BuildUp(this);
        OwnerId = ownerId;
        CharacterId = characterId;
        MaxHealth = 100f; //Todo: Config
        Health = MaxHealth;
        IsBot = isBot;
    }

    private void OnDestroy() {
        _SignalBus?.FireSignal(new GameCameraTargetsChangeSignal(this, false));
        _SignalBus?.UnSubscribeFromAll(this);
        Characters.Remove(this);
    }

    public void Kill(Damage damage) {
        Dead = true;
        Debug.Log($"Player {OwnerId} character {CharacterId} dead.");
        Destroy(gameObject); //ToDo: something different
        _SignalBus?.FireSignal(new CharacterDeathSignal(damage));
        PlayeHitSound(DeathAudioEffects);
    }
}
