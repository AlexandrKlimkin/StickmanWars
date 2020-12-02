using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using KlimLib.Timers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class MainPanel : UIPanel {
        [SerializeField]
        private TimerController _GameTimer;

        public override void Setup() {
            base.Setup();
            _GameTimer.StartAscendingTimer();
        }
    }
}