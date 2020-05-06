using KlimLib.TaskQueueLib;
using System.Collections.Generic;
using Core.Initialization;

namespace Core.Services.SceneManagement {
    public class MapSelectionLoadingParameters : SceneLoadingParameters {
        public override List<Task> LoadingTasks => InitializationParameters.MapSelectionLoadTasks;
        public override List<Task> UnloadingTasks => InitializationParameters.MapSelectionUnloadTasks;
    }
}