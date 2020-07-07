using Character.Control;
using Com.LuisPedroFonseca.ProCamera2D;
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

        private ProCamera2D Camera2D => _Camera2D == null ? ContainerHolder.Container.Resolve<ProCamera2D>() : _Camera2D;
        private readonly ProCamera2D _Camera2D;
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
            _SignalBus.FireSignal(new CharacterSpawnedSignal(unit));
            if(Camera2D != null) {
                Camera2D.AddCameraTarget(unit.transform);
            }
            return unit;
        }


        private void SetupLocalPlayerComponents(CharacterUnit character, int deviceId) {
            var playerController = character.GetComponent<PlayerController>();
            playerController.Id = deviceId;
        }
    }
}