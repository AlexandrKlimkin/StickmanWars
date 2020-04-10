using System.Collections.Generic;
using Core.Initialization.Base;
using Core.Initialization.Game;
using Core.Services.Game;
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
            };

        public static List<Task> GameTasks => new List<Task> {
            new RegisterAndLoadServiceTask<CharacterSpawnService>(),

            new WaitForAwakesTask(),
            new GameCameraSpawnTask(),
        };
    }
}