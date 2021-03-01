using KlimLib.TaskQueueLib;
using System.Collections.Generic;
using System.Linq;
using Core.Initialization;

namespace Core.Services.SceneManagement {
    public class LobbyLoadingParameters : SceneLoadingParameters {
        public override List<Task> LoadingTasks => InitializationParameters.LobbyLoadTasks;
        public override List<Task> UnloadingTasks => InitializationParameters.LobbyUnloadTasks;
    }
}
