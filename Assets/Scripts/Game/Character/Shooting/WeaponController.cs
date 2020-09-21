using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Character.CloseCombat;
using Core.Audio;
using InputSystem;
using UnityDI;
using UnityEngine;

namespace Character.Shooting {
    public class WeaponController : MonoBehaviour {
        [Dependency]
        private readonly AudioService _AudioService;

        public Transform NearArmTransform;
        public Transform NearArmShoulder;
        public Transform NearArmWeaponTransform;
        public Transform NeckWeaponTransform;
        public Weapon MainWeapon;
        public Weapon Vehicle;
        public MeleeAttack MeleeAttack;

        public bool HasMainWeapon => MainWeapon != null;
        public bool HasVehicle => Vehicle != null;

        public event Action OnPressFire;
        public event Action OnHoldFire;
        public event Action OnReleaseFire;
        public event Action<Weapon> OnWeaponEquiped;
        public event Action<Weapon> OnVehicleEquiped;
        public event Action<Weapon> OnWeaponThrowed;
        public event Action<Weapon> OnVehicleThrowed;

        public CharacterUnit Owner { get; private set; }
        public WeaponPicker WeaponPicker { get; private set; }

        public Vector2 AimPosition { get; private set; }

        public string ThrowSound;
        public string PickUpSound;

        public bool MeleeAttacking => MeleeAttack.Attacking && MainWeapon == null;

        private void Awake() {
            Owner = GetComponent<CharacterUnit>();
            WeaponPicker = GetComponentInChildren<WeaponPicker>();
        }

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            MainWeapon?.PickableItem.PickUp(Owner);
            TryPickUpWeapon(MeleeAttack);
            Vehicle?.PickableItem.PickUp(Owner);
        }

        private void Update() {
            if(MainWeapon == null) {
                MeleeAttack?.InputProcessor.Process();
            }
            else  {
                MainWeapon.InputProcessor.Process();
            }
            Vehicle?.InputProcessor.Process();

        }

        public void SetAimPosition(Vector2 position) {
            SetWeaponedHandPosition(position);
        }

        public void SetWeaponedHandPosition(Vector2 position) {
            AimPosition = position;
            NearArmTransform.position = AimPosition;
        }

        public void HoldFire() {
            OnHoldFire?.Invoke();
        }

        public void PressFire() {
            OnPressFire?.Invoke();
        }

        public void ReleaseFire() {
            OnReleaseFire?.Invoke();
        }

        public void ThrowOutMainWeapon() {
            if (!HasMainWeapon) return;
                ThrowOutMainWeapon(MainWeapon.Stats.MaxThrowStartSpeed, -360f);
        }

        public void ThrowOutMainWeapon(float startSpeed, float angularVel) {
            if (!HasMainWeapon) return;
            Vector2 dir = (AimPosition - MainWeapon.WeaponView.ShootTransform.position.ToVector2());
            dir.Normalize();
            ThrowOutMainWeapon(dir * startSpeed, angularVel);
        }

        public void ThrowOutMainWeapon(Vector2 startSpeed, float angularVel) {
            if (!HasMainWeapon) return;
            MainWeapon.ThrowOut(Owner, startSpeed, angularVel);
            var weapon = MainWeapon;
            MainWeapon = null;
            PlaySound(ThrowSound);
            OnWeaponThrowed?.Invoke(weapon);
        }

        public void ThrowOutVehicle() {
            if (!HasVehicle) return;
            ThrowOutVehicle(Vehicle.Stats.MaxThrowStartSpeed, -360f);
        }

        public void ThrowOutVehicle(float startSpeed, float angularVel) {
            if (!HasVehicle) return;
            Vector2 dir = (AimPosition - Vehicle.WeaponView.ShootTransform.position.ToVector2());
            dir.Normalize();
            ThrowOutVehicle(dir * startSpeed, angularVel);
        }

        public void ThrowOutVehicle(Vector2 startSpeed, float angularVel) {
            if (!HasVehicle) return;
            Vehicle.ThrowOut(Owner, startSpeed, angularVel);
            var vehicle = Vehicle;
            Vehicle = null;
            PlaySound(ThrowSound);
            OnVehicleThrowed?.Invoke(vehicle);
        }

        private void PlaySound(string soundName) {
            if (string.IsNullOrEmpty(soundName))
                return;
            _AudioService.PlaySound3D(soundName, false, false, transform.position);
        }

        public void TryPickUpWeapon(Weapon weapon) {
            if (weapon.ItemType == ItemType.Weapon) {
                if (HasMainWeapon)
                    return;
                if (weapon.PickUp(Owner)) {
                    MainWeapon = weapon;
                    PlaySound(PickUpSound);
                    OnWeaponEquiped?.Invoke(weapon);
                }
            } else if (weapon.ItemType == ItemType.Vehicle) {
                if (HasVehicle)
                    return;
                if (weapon.PickUp(Owner)) {
                    Vehicle = weapon;
                    PlaySound(PickUpSound);
                    OnVehicleEquiped?.Invoke(weapon);
                }
            } else if(weapon.ItemType == ItemType.MeleeAttack) {
                weapon.PickUp(Owner);
            }
        }

        public void SubscribeWeaponOnEvents(Weapon weapon) {
            OnHoldFire += weapon.InputProcessor.ProcessHold;
            OnPressFire += weapon.InputProcessor.ProcessPress;
            OnReleaseFire += weapon.InputProcessor.ProcessRelease;
        }

        public void UnSubscribeWeaponOnEvents(Weapon weapon) {
            OnHoldFire -= weapon.InputProcessor.ProcessHold;
            OnPressFire -= weapon.InputProcessor.ProcessPress;
            OnReleaseFire -= weapon.InputProcessor.ProcessRelease;
        }

        private void OnDestroy() {
            ThrowOutMainWeapon(Owner.Rigidbody2D.velocity, UnityEngine.Random.Range(-360f, 360f));
            ThrowOutVehicle();
        }
    }
}