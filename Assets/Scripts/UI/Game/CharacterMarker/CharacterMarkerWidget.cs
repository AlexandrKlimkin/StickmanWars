using System.Collections;
using System.Collections.Generic;
using UI.Markers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Markers {
    public class CharacterMarkerWidget : MarkerWidget<CharacterMarkerData> {
        [SerializeField]
        private Slider _HPSlider;
        [SerializeField]
        private Slider _VehicleSlider;
        [SerializeField]
        private Text _AmmoText;

        protected override void HandleData(CharacterMarkerData data) {
            _HPSlider.value = data.NormilizedHealth;
            _AmmoText.gameObject.SetActive(data.HasWeapon);
            if (data.HasWeapon) {
                _AmmoText.text = $"{data.Ammo}";
            }
            _VehicleSlider.gameObject.SetActive(data.HasVehicle);
            if (data.HasVehicle) {
                _VehicleSlider.value = (float)data.VehicleAmmo / data.VehicleMaxAmmo;
            }
        }
    }
}
