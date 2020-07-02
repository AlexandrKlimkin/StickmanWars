using Character.Shooting;
using Game.CameraTools;
using KlimLib.SignalBus;
using UI.Markers;
using UnityDI;
using UnityEngine;

namespace RC.UI.Markers {
    public class ItemMarkerProvider : MarkerProvider<ItemMarkerWidget, ItemMarkerData> {
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly MainCamera _MainCamera;

        private Weapon _Weapon;
        private bool _RefreshPreview = true;

        private void Awake() {
            _Weapon = GetComponent<Weapon>();
            OnVisibilityChanged += OnVisibileBecome;
        }

        public override bool GetVisibility() {
            return _Weapon.PickableItem.Owner == null;
        }

        protected override void RefreshData(ItemMarkerData data) {
            base.RefreshData(data);
            data.WeaponId = _Weapon.Id;
            _RefreshPreview = false;
        }

        private void OnVisibileBecome(MarkerProvider provider) {
            if (Visible)
                _RefreshPreview = true;
        }
    }
}