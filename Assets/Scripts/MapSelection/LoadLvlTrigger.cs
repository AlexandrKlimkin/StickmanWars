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
        [SerializeField] 
        private bool _Tube;
        [SerializeField]
        private Animation _TubeAnim;

        protected override LoadLvlTriggerInteractionSignal CreateSignal(CharacterUnit characterUnit, bool enter) {
            if (_Tube && !_TubeAnim.isPlaying)
            {
                _TubeAnim.Play("Tube");
            }
            else if (_Tube)
            {
                _TubeAnim.Stop();
            }
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