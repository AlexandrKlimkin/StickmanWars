using System.Collections.Generic;
using KlimLib.TaskQueueLib;

namespace Core.Initialization {
    public class MapSelectionInitializer : InitializerBase {
        protected override List<Task> SpecialTasks => InitializationParameters.MapSelectionTasks;
    }
}