using System.Collections.Generic;
using System.Linq;
using KlimLib.TaskQueueLib;

namespace Core.Initialization {
    public class MapSelectionInitializer : InitializerBase {
        protected override List<Task> SpecialTasks =>
            InitializationParameters.BaseGameTasks.Concat(
                InitializationParameters.MapSelectionLoadTasks).ToList();
    }
}