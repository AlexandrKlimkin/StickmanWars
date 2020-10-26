﻿using System;
using System.Collections;
using System.Collections.Generic;
using Character.CloseCombat;
using Character.Health;
using Com.LuisPedroFonseca.ProCamera2D;
using Core.Audio;
using Items;
using UnityDI;
using UnityEngine;

namespace Character.Shooting {
    public abstract class Weapon : MonoBehaviour {
        public abstract ItemType ItemType { get; }
        public virtual WeaponReactionType WeaponReaction => WeaponReactionType.Fire;

        public WeaponView WeaponView { get; protected set; }
        public PickableItem PickableItem { get; protected set; }

        public WeaponInputProcessor InputProcessor { get; private set; }

        [SerializeField]
        protected WeaponConfig _Stats;
        public WeaponPickupType PickupType;
        [SerializeField]
        private string _Id;
        public string Id => _Id;

        public WeaponConfig Stats => _Stats;

        public List<string> ShotSoundEffects;

        public event Action OnNoAmmo;

        public string ShotCameraShakePresetName;

        [Dependency]
        protected readonly AudioService _AudioService;

        public virtual void PerformShot() {
            if (InputProcessor.CurrentMagazine <= 0)
                OnNoAmmo?.Invoke();
            if (ProCamera2DShake.Instance != null && !string.IsNullOrEmpty(ShotCameraShakePresetName))
                ProCamera2DShake.Instance.Shake(ShotCameraShakePresetName);
            PlayShotSound();
        }

        private void PlayShotSound() {
            if (ShotSoundEffects == null || ShotSoundEffects.Count == 0)
                return;
            _AudioService.PlaySound3D(ShotSoundEffects[UnityEngine.Random.Range(0, ShotSoundEffects.Count)], false, false, transform.position);
        }

        protected virtual bool UseThrowForce => true;

        protected virtual void Awake() {
            PickableItem = GetComponent<PickableItem>();
        }

        protected virtual void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
            WeaponView = PickableItem?.ItemView as WeaponView;
            WeaponsInfoContainer.AddWeapon(this);
        }

        public virtual bool PickUp(CharacterUnit owner) {
            var pickedUp = false;
            if (PickableItem != null)
                pickedUp = PickableItem.PickUp(owner);
            else
                pickedUp = this is MeleeAttack;
            if (pickedUp) {
                if (WeaponReaction == WeaponReactionType.Fire)
                    owner?.WeaponController?.SubscribeWeaponOnEvents(this);
                else if (WeaponReaction == WeaponReactionType.Jump)
                    owner?.MovementController?.SubscribeWeaponOnEvents(this);
            }
            return pickedUp;
        }

        public virtual void ThrowOut(CharacterUnit owner, Vector2? startVelocity = null, float? angularVel = null) {
            if (WeaponReaction == WeaponReactionType.Fire)
                owner?.WeaponController?.UnSubscribeWeaponOnEvents(this);
            else if (WeaponReaction == WeaponReactionType.Jump)
                owner?.MovementController?.UnSubscribeWeaponOnEvents(this);
            PickableItem.ThrowOut(startVelocity, angularVel);
        }

        public virtual void SetInputProcessor(WeaponInputProcessor inputProcessor) {
            InputProcessor = inputProcessor;
        }

        protected virtual Damage GetDamage() {
            return new Damage(PickableItem.Owner?.OwnerId, null, _Stats.Damage);
        }

        protected virtual void Enable() {
            WeaponsInfoContainer.AddWeapon(this);
        }

        protected virtual void OnDisable() {
            WeaponsInfoContainer.RemoveWeapon(this);
        }
    }

    public enum ItemType {
        Weapon,
        Vehicle,
        MeleeAttack,
    }

    public enum WeaponReactionType {
        Fire,
        Jump,
    }

    public enum WeaponPickupType {
        ArmNear,
        Neck,
        None,
    }
}