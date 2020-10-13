using System.Collections;
using System.Collections.Generic;
using UI.Markers;
using UnityEngine;

namespace UI.Game.Markers {
    public class CharacterMarkerData : MarkerData {
        public float NormilizedHealth;
        public bool HasWeapon;
        public int Ammo;
        public int MaxAmmo;
        public bool IsBot;

        public bool HasVehicle;
        public int VehicleAmmo;
        public int VehicleMaxAmmo;
        public float NormilizedStartVelocity;
    }
}
