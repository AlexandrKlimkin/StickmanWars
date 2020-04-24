﻿using System.Collections.Generic;
using System.Linq;
using Core.Initialization.Base;
using Core.Initialization.Game;
using Core.Services;
using Core.Services.Controllers;
using Core.Services.Game;
using Core.Services.SceneManagement;
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
                new RegisterAndLoadServiceTask<SceneManagerService>(),
                new RegisterAndLoadServiceTask<ControllersStatusService>(),
            };

        public static List<Task> MapSelectionTasks =>
            new List<Task>() {
                new RegisterAndLoadServiceTask<MatchService>(),
                new RegisterAndLoadServiceTask<PlayersConnectionService>(),
            }.Concat(BaseGameTasks).ToList();

        public static List<Task> BaseGameTasks => new List<Task> {
            new WaitForAwakesTask(),
            new RegisterAndLoadServiceTask<CharacterSpawnService>(),
            new GameCameraSpawnTask(),
        };
    }
}