using Game.Match;
using KlimLib.TaskQueueLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Core.Initialization.Game {
    public class DisableObjectsIfGameWithBotsTask : AutoCompletedTask {
        [Dependency]
        private readonly MatchData _MatchData;

        protected override void AutoCompletedRun() {
            if (_MatchData.Players?.FirstOrDefault(_ => _.IsBot) == null)
                return;
            var objectsToDisable = GameObject.FindGameObjectsWithTag("DisableIfBots");
            objectsToDisable.ForEach(_ => _.SetActive(false));
        }
    }
}
