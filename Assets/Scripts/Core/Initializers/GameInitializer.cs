using System.Collections;
using System.Collections.Generic;
using Core.Initialization.Game;
using KlimLib.TaskQueueLib;
using UnityEngine;

namespace Core.Initialization {
    public class GameInitializer : InitializerBase {
        protected override List<Task> SpecialTasks => InitializationParameters.BaseGameTasks;
    }
}