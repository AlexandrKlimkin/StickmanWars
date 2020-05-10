using System.Collections;
using KlimLib.ResourceLoader;
using UI.Markers;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace MapSelection.UI {
    public class CharacterSelectionMarkerWidget : MarkerWidget<CharacterSelectionMarkerData> {
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;

        [SerializeField]
        private Transform _StatesTransform;
        [SerializeField]
        private Image _Preview;
        [SerializeField]
        private UIInterpolatorAnimation _LeftArrow;
        [SerializeField]
        private UIInterpolatorAnimation _RightArrow;

        private bool _CachedPlayerConnected;

        protected override void Start() {
            base.Start();
            ContainerHolder.Container.BuildUp(this);
            SetState("PressButtonState");
        }

        protected override void HandleData(CharacterSelectionMarkerData data) {
            if (_CachedPlayerConnected != data.PlayerConnected)
                SetState(data.PlayerConnected ? "CharacterSelectionState" : "PressButtonState");
            _CachedPlayerConnected = data.PlayerConnected;
            if (data.ChangePreview) {
                _Preview.sprite = _ResourceLoader.LoadResource<Sprite>(data.PreviewPath);
                if (data.Left)
                    _LeftArrow.Play();
                if (data.Right)
                    _RightArrow.Play();
            }
        }

        private void SetState(string name) {
            for (var i = 0; i < _StatesTransform.childCount; i++) {
                var child = _StatesTransform.GetChild(i);
                child.gameObject.SetActive(child.name == name);
            }
        }
    }
}