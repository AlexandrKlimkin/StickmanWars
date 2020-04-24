using KlimLib.TaskQueueLib;
using System.Collections.Generic;

namespace Core.Services.SceneManagement {
    public class MapSelectionLoadingParameters : SceneLoadingParameters {
        public override List<Task> LoadingTasks { get; }
        public override List<Task> UnloadingTasks { get; }
    }
}