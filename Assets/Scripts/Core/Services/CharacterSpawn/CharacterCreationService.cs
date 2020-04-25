using Character.Control;
using Game.CameraTools;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using Tools.Services;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class CharacterCreationService : ILoadableService, IUnloadableService {

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;

        public void Load() { }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        public Unit CreateCharacter(string characterId, bool isLocalPlayer, int deviceId, Vector3 pos) {
            var path = Path.Resources.CharacterPath(characterId);
            var unit = _ResourceLoader.LoadResourceOnScene<Unit>(path, pos, Quaternion.identity);
            ContainerHolder.Container.BuildUp(unit);
            if (isLocalPlayer) {
                SetupLocalPlayerComponents(unit, deviceId);
            }
            _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(unit, true));
            return unit;
        }

        private void SetupLocalPlayerComponents(Unit character, int deviceId) {
            var playerController = character.GetComponent<PlayerController>();
            playerController.Id = deviceId;
        }
    }
}