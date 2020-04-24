using System.Collections;
using System.Collections.Generic;
using Core.Services.SceneManagement;
using Game.LevelSpecial;
using Game.Match;
using UnityDI;
using UnityEngine;

namespace MapSelection {
    public class LoadLvlTrigger : UnitTrigger {

        public SceneType SceneToLoad;

        [Dependency]
        private readonly SceneManagerService _SceneManager;
        [Dependency]
        private readonly MatchData _MatchData;

        private void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        public override void OnUnitEnterTheTrigger(Unit unit) {
            base.OnUnitEnterTheTrigger(unit);
            if(!AllSpawnedUnitsInTrigger)
                return;
            _SceneManager.LoadScene(SceneToLoad);
        }

        public override void OnUnitExitTheTrigger(Unit unit) {
            base.OnUnitExitTheTrigger(unit);
        }

        private bool AllSpawnedUnitsInTrigger => UnitsInside.Count >= _MatchData.Players.Count;

    }
}
