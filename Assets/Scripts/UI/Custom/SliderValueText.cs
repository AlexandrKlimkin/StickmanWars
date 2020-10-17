using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour {
    public Slider Slider;
    private Text _Text;

    private void Awake() {
        _Text = GetComponent<Text>();
    }

    private void Start() {
        if (_Text == null || Slider == null)
            return;
        Slider.onValueChanged.AddListener(OnSliderValueUpdate);
        OnSliderValueUpdate(Slider.value);
    }

    private void OnSliderValueUpdate(float value) {
        _Text.text = value.ToString();
    }

    private void OnDestroy() {
        Slider.onValueChanged.RemoveAllListeners();
    }
}