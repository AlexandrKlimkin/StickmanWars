using System.Collections.Generic;
using Core.Initialization;
using KlimLib.TaskQueueLib;

namespace Core.Services.SceneManagement {
    public class GameLoadingParameters : SceneLoadingParameters {
        public override List<Task> LoadingTasks => InitializationParameters.GameLoadTasks;
        public override List<Task> UnloadingTasks { get; }
    }
}
