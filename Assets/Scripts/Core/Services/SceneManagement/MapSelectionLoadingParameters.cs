using KlimLib.TaskQueueLib;
using System.Collections.Generic;
using System.Linq;
using Core.Initialization;

namespace Core.Services.SceneManagement {
    public class MapSelectionLoadingParameters : SceneLoadingParameters {
        public override List<Task> LoadingTasks => InitializationParameters.BaseGameTasks
            .Concat(InitializationParameters.MapSelectionLoadTasks).ToList();
        public override List<Task> UnloadingTasks => InitializationParameters.MapSelectionUnloadTasks;
    }
}