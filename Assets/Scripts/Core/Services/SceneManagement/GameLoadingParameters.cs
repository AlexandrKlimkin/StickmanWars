using System.Collections.Generic;
using KlimLib.TaskQueueLib;

namespace Core.Services.SceneManagement {
    public class GameLoadingParameters : SceneLoadingParameters{
        public override List<Task> LoadingTasks { get; }
        public override List<Task> UnloadingTasks { get; }
    }
}
