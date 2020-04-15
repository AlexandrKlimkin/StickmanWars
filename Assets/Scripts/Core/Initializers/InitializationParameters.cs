using System.Collections.Generic;
using System.Linq;
using Core.Initialization.Base;
using Core.Initialization.Game;
using Core.Services.Controllers;
using Core.Services.Game;
using Game.Match;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using KlimLib.TaskQueueLib;

namespace Core.Initialization {
    public static class InitializationParameters {
        public static List<Task> BaseTasks =>
            new List<Task> {
                new ContainerInitializationTask(),
                new BaseServiceInitializationTask<SignalBus, SignalBus>(),
                new BaseServiceInitializationTask<IResourceLoaderService, ResourceLoaderService>(),
                new UnityEventProviderRegisterTask(),
                new RegisterAndLoadServiceTask<ControllersStatusService>(),
            };

        public static List<Task> MapSelectionTasks =>
            new List<Task>() {
                new RegisterAndLoadServiceTask<MatchService>(),
                new RegisterAndLoadServiceTask<PlayersConnectionService>(),
            }.Concat(GameTasks).ToList();

        public static List<Task> GameTasks => new List<Task> {
            new RegisterAndLoadServiceTask<CharacterSpawnService>(),

            new WaitForAwakesTask(),
            new GameCameraSpawnTask(),
        };
    }
}