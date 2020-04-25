using System.Collections;
using System.Collections.Generic;
using KlimLib.Timers;
using UnityDI;
using UnityEngine;

namespace MapSelection {
    public class MapSelectionUIManager : MonoBehaviour {
        public TimerController LoadLevelTimer;

        private void OnDestroy() {
            ContainerHolder.Container.Unregister(GetType());
        }
    }
}