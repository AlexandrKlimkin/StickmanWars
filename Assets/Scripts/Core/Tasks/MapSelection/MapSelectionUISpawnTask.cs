using KlimLib.ResourceLoader;
using KlimLib.TaskQueueLib;
using MapSelection;
using UnityDI;

namespace Core.Initialization.MapSelection {
    public class MapSelectionUISpawnTask : AutoCompletedTask {
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;

        protected override void AutoCompletedRun() {
            var ui = _ResourceLoader.LoadResourceOnScene<MapSelectionUIManager>(Path.Resources.MapSelectionUI);
            ContainerHolder.Container.RegisterInstance(ui);
            ContainerHolder.Container.BuildUp(ui);
        }
    }
}
