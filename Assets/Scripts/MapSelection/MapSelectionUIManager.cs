using System.Collections;
using System.Collections.Generic;
using KlimLib.Timers;
using UnityDI;
using UnityEngine;

namespace MapSelection {
    public class MapSelectionUIManager : MonoBehaviour {
        public TimerController LoadLevelTimer;
        public GameObject SettingsPanel;

        private void OnDestroy() {
            ContainerHolder.Container.Unregister(GetType());
        }

        public void ShowSettings(bool enable) {
            SettingsPanel.SetActive(enable);
        }
    }
}