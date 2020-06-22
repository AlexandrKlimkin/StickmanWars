using Character.Control;
using Game.CameraTools;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using UI.Game.Markers;
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

        public CharacterUnit CreateCharacter(string characterId, byte playerId, bool isLocalPlayer, int deviceId, Vector3 pos) {
            var path = Path.Resources.CharacterPath(characterId);
            var unit = _ResourceLoader.LoadResourceOnScene<CharacterUnit>(path, pos, Quaternion.identity);
            if (isLocalPlayer) {
                SetupLocalPlayerComponents(unit, deviceId);
            }
            unit.Initialize(playerId, characterId);

            //unit.gameObject.AddComponent<CharacterMarkerProvider>();
            _SignalBus.FireSignal(new CharacterSpawnedSignal(unit));
            _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(unit, true));
            return unit;
        }

        private void SetupLocalPlayerComponents(CharacterUnit character, int deviceId) {
            var playerController = character.GetComponent<PlayerController>();
            playerController.Id = deviceId;
        }
    }
}