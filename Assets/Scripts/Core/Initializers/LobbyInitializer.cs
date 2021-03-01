using System.Collections;
using System.Collections.Generic;
using KlimLib.TaskQueueLib;
using UnityEngine;

namespace Core.Initialization {
    public class LobbyInitializer : InitializerBase {
        protected override List<Task> SpecialTasks => InitializationParameters.LobbyLoadTasks;
    }
}
