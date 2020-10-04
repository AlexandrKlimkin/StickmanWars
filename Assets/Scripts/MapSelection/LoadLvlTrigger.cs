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

        protected override LoadLvlTriggerInteractionSignal CreateSignal(CharacterUnit characterUnit, bool enter) {
            return new LoadLvlTriggerInteractionSignal(characterUnit, enter, UnitsInside.Count, _SceneType);
        }
    }

    public struct LoadLvlTriggerInteractionSignal {
        public CharacterUnit CharacterUnit;
        public bool Enter;
        public int TotalUnitsInsde;
        public SceneType SceneType;

        public LoadLvlTriggerInteractionSignal(CharacterUnit characterUnit, bool enter, int unitsInside, SceneType sceneType) {
            this.CharacterUnit = characterUnit;
            this.Enter = enter;
            this.TotalUnitsInsde = unitsInside;
            this.SceneType = sceneType;
        }
    }
}