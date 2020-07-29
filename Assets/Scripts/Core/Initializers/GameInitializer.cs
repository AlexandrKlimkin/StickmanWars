using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Control;
using Core.Initialization.Game;
using Core.Services.Game;
using Game.Match;
using KlimLib.SignalBus;
using KlimLib.TaskQueueLib;
using UnityDI;
using UnityEngine;

namespace Core.Initialization {
    public class GameInitializer : InitializerBase {
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly MatchService _MatchService;
        [Dependency]
        private readonly PlayersConnectionService _PlayersConnectionService;
        [Dependency]
        private readonly GameManagerService _GameManagerService;

        private List<CharacterUnit> _Units;

        protected override List<Task> SpecialTasks =>
            InitializationParameters.BaseGameTasks.Concat(
                InitializationParameters.GameLoadTasks
            ).ToList();

        private void Start() {
            if(!_WasInitialized)
                return;
            ContainerHolder.Container.BuildUp(this);
            AddExistingCharactersInMap();
        }

        private void AddExistingCharactersInMap() {
            ContainerHolder.Container.BuildUp(this);
            _Units = FindObjectsOfType<CharacterUnit>().ToList();
            byte i = 0;
            foreach (var unit in _Units) {
                var player = new PlayerData(i, i.ToString(), false, i, unit.CharacterId);
                //_GameManagerService.AddPlayerOnMap(unit.CharacterId, unit.Is);
                Destroy(unit.gameObject);
                i++;
            }
        }

        private void OnDestroy() {
            _SignalBus?.UnSubscribeFromAll(this);
        }
    }
}