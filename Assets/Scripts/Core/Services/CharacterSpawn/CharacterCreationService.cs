using Character.Control;
using Com.LuisPedroFonseca.ProCamera2D;
using Game.CameraTools;
using Game.Match;
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

        public CharacterUnit CreateCharacter(PlayerData playerData, bool isLocalPlayer, int deviceId, Vector3 pos) {
            var path = Path.Resources.CharacterPath(playerData.CharacterId);
            var unit = _ResourceLoader.LoadResourceOnScene<CharacterUnit>(path, pos, Quaternion.identity);
            if (isLocalPlayer) {
                SetupLocalPlayerComponents(unit, playerData, deviceId);
            }
            unit.Initialize(playerData.PlayerId, playerData.CharacterId);
            _SignalBus.FireSignal(new CharacterSpawnedSignal(unit));
            if(Camera2D != null) {
                Camera2D.AddCameraTarget(unit.transform);
            }
            return unit;
        }

        private void SetupLocalPlayerComponents(CharacterUnit character, PlayerData playerData, int deviceId) {
            if (playerData.IsBot) {
                //ToDo: Add AI Components
            } else {
                var playerController = character.gameObject.AddComponent<PlayerController>();
                playerController.Id = deviceId;
            }
        }
    }
}