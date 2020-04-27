using KlimLib.ResourceLoader;
using KlimLib.TaskQueueLib;
using MapSelection;
using UnityDI;

namespace Core.Initialization.Game {
    public class GameUISpawnTask : AutoCompletedTask {
        private readonly IResourceLoaderService _ResourceLoader;

        protected override void AutoCompletedRun() {
            var ui = _ResourceLoader.LoadResourceOnScene<UIManager>(Path.Resources.GameUI);
            ContainerHolder.Container.RegisterInstance(ui);
            ContainerHolder.Container.BuildUp(ui);
        }
    }
}
