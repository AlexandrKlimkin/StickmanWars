using System.Collections;
using System.Collections.Generic;
using Core.Services.SceneManagement;
using Game.LevelSpecial;
using Game.Match;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace MapSelection {
    public class LoadLvlTrigger : UnitTriggerSignalBroadcaster<LoadLvlTriggerInteractionSignal> {
        [SerializeField]
        private SceneType _SceneType;

        protected override LoadLvlTriggerInteractionSignal CreateSignal(Unit unit, bool enter) {
            return new LoadLvlTriggerInteractionSignal(unit, enter, UnitsInside.Count, _SceneType);
        }
    }

    public struct LoadLvlTriggerInteractionSignal {
        public Unit Unit;
        public bool Enter;
        public int TotalUnitsInsde;
        public SceneType SceneType;

        public LoadLvlTriggerInteractionSignal(Unit unit, bool enter, int unitsInside, SceneType sceneType) {
            this.Unit = unit;
            this.Enter = enter;
            this.TotalUnitsInsde = unitsInside;
            this.SceneType = sceneType;
        }
    }
}