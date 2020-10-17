using Core.Services.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings {
    public class BotsCountSettingsSlider : MonoBehaviour {
        [Dependency]
        private readonly PlayersConnectionService _PlayersConnectionService;
        private Slider _Slider;

        private void Awake() {
            _Slider = GetComponent<Slider>();
        }

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _Slider.onValueChanged.AddListener(OnSliderChange);
            _Slider.value = _PlayersConnectionService.MaxBotsCount;
        }

        private void OnSliderChange(float value) {
            var count = (int)value;
            _PlayersConnectionService.MaxBotsCount = count;
        }

        private void OnDestroy() {
            _Slider.onValueChanged.RemoveAllListeners();
        }
    }
}
