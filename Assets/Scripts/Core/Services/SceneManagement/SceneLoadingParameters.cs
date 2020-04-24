using System.Collections.Generic;
using UnityEngine;
using KlimLib.TaskQueueLib;

namespace Core.Services.SceneManagement {
    public abstract class SceneLoadingParameters {

        public abstract List<Task> LoadingTasks { get; }
        public abstract List<Task> UnloadingTasks { get; }

        public virtual void BeforeLoad() { }
        public virtual void AfterLoad() { }
        public virtual void BeforeUnload() { }
        public virtual void AfterUnload() { }
    }
}