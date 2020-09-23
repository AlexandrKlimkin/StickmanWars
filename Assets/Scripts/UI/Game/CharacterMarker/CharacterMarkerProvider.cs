using Character.Shooting;
using System.Collections;
using System.Collections.Generic;
using UI.Markers;
using UnityEngine;

namespace UI.Game.Markers {
    public class CharacterMarkerProvider : MarkerProvider<CharacterMarkerWidget, CharacterMarkerData> {
        protected CharacterUnit _CharacterUnit;

        protected virtual void Awake() {
            _CharacterUnit = GetComponentInParent<CharacterUnit>();
        }

        public override bool GetVisibility() {
            return _CharacterUnit && !_CharacterUnit.Dead;
        }

        protected override void RefreshData(CharacterMarkerData data) {
            base.RefreshData(data);
            data.NormilizedHealth = _CharacterUnit.NormilizedHealth;
            data.HasWeapon = _CharacterUnit.WeaponController.HasMainWeapon;
            if (data.HasWeapon) {
                data.Ammo = _CharacterUnit.WeaponController.MainWeapon.InputProcessor.CurrentMagazine;
                data.MaxAmmo = _CharacterUnit.WeaponController.MainWeapon.Stats.Magazine;
            }
            data.HasVehicle = _CharacterUnit.WeaponController.HasVehicle;
            if (data.HasVehicle) {
                data.VehicleAmmo = _CharacterUnit.WeaponController.Vehicle.InputProcessor.CurrentMagazine;
                data.VehicleMaxAmmo = _CharacterUnit.WeaponController.Vehicle.Stats.Magazine;
            }
            var normilizedFireForceStartVelocity = 0f;
            if (_CharacterUnit.WeaponController.HasMainWeapon) {
                if (_CharacterUnit.WeaponController.MainWeapon.InputProcessor is FireForceProcessor) {
                    normilizedFireForceStartVelocity = ((FireForceProcessor)(_CharacterUnit.WeaponController.MainWeapon.InputProcessor)).NormilizedForce;
                }
            }
            data.NormilizedStartVelocity = normilizedFireForceStartVelocity;
        }
    }
}
