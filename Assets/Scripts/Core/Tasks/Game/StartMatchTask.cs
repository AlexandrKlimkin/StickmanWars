using Core.Services.Game;
using KlimLib.TaskQueueLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;

namespace Core.Initialization.Game {
    public class StartMatchTask : AutoCompletedTask {
        [Dependency]
        private readonly GameManagerService _GameManagerService;

        protected override void AutoCompletedRun() {
            _GameManagerService.StartMatch();
        }
    }
}
