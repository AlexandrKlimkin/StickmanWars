using System.Collections.Generic;
using System.Linq;
using Core.Initialization.Base;
using Core.Initialization.Game;
using Core.Initialization.MapSelection;
using Core.Services;
using Core.Services.Controllers;
using Core.Services.Game;
using Core.Services.MapSelection;
using Core.Services.SceneManagement;
using Game.Match;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using KlimLib.TaskQueueLib;
using MapSelection;
using UI.Markers;

namespace Core.Initialization {
    public static class InitializationParameters {
        public static List<Task> BaseTasks =>
            new List<Task> {
                new ContainerInitializationTask(),
                new BaseServiceInitializationTask<SignalBus, SignalBus>(),
                new BaseServiceInitializationTask<IResourceLoaderService, ResourceLoaderService>(),
                new UnityEventProviderRegisterTask(),
                new RegisterAndLoadServiceTask<SceneManagerService>(),
                new RegisterAndLoadServiceTask<ControllersStatusService>(),
                new RegisterAndLoadServiceTask<MarkerService>(),
            };

        public static List<Task> MapSelectionLoadTasks =>
            BaseGameTasks
                .Concat(
                    new List<Task> {
                        new RegisterAndLoadServiceTask<PlayersConnectionService>(),
                        new RegisterAndLoadServiceTask<CharacterSelectionService>(),
                        new MapSelectionUISpawnTask(),
                        new RegisterAndLoadServiceTask<GameLevelLoadService>(),
                        //new GameCameraSpawnTask(),
                    })
                .ToList();

        public static List<Task> MapSelectionUnloadTasks => new List<Task>() {
            new UnregisterAndUnloadServiceTask<GameLevelLoadService>(),
            //new UnregisterAndUnloadServiceTask<MarkerService>(),
            new UnregisterAndUnloadServiceTask<CharacterSelectionService>(),
            //new UnregisterAndUnloadServiceTask<PlayersConnectionService>(),
        };

        public static List<Task> BaseGameTasks =>
            new List<Task> {
                new WaitForAwakesTask(),
                new RegisterAndLoadServiceTask<MatchService>(),
                new RegisterAndLoadServiceTask<CharacterCreationService>(),
            };

        public static List<Task> GameLoadTasks => new List<Task> {
            new WaitForAwakesTask(),
            new GameUISpawnTask(),
            new RegisterAndLoadServiceTask<GameManagerService>(),
            new GameCameraSpawnTask(),
            new RegisterAndLoadServiceTask<RespawnModeService>(),
            new RegisterAndLoadServiceTask<ObjectsSpawnService>(),
        };

        public static List<Task> GameUnloadTasks => new List<Task>() {
            new UnregisterAndUnloadServiceTask<ObjectsSpawnService>(),
            new UnregisterAndUnloadServiceTask<RespawnModeService>(),
            new UnregisterAndUnloadServiceTask<GameManagerService>(),
            new UnregisterAndUnloadServiceTask<MatchService>(),
            new UnregisterAndUnloadServiceTask<PlayersConnectionService>(),
        };
    }
}