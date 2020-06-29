﻿using Game.CameraTools;
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

        public override bool GetVisibility() {
            return true;
        }

        protected override void RefreshData(ItemMarkerData data) {
            base.RefreshData(data);
        }


    }
}