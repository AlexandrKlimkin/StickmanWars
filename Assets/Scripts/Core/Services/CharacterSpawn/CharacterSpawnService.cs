using Character.Control;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using Tools.Services;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class CharacterSpawnService : ILoadableService, IUnloadableService {

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoader;
        [Dependency]
        private readonly PlayersSpawnSettings _PlayersSpawnSettings;

        public void Load() {
            _SignalBus.Subscribe<PlayerConnectedSignal>(OnPlayerConnected, this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void OnPlayerConnected(PlayerConnectedSignal signal) {
            var spawnPoint = _PlayersSpawnSettings.PlayerSpawnPoints[signal.PlayerData.PlayerId].Point;
            SpawnCharacter(signal.PlayerData.CharacterId, true, signal.DeviceId, spawnPoint.position, spawnPoint.rotation);
        }

        private void SpawnCharacter(string characterId, bool isLocalPlayer, int deviceId, Vector3 pos, Quaternion rot) {
            var path = Path.Resources.CharacterPath(characterId);
            var unit = _ResourceLoader.LoadResourceOnScene<Unit>(path, pos, rot);
            if (isLocalPlayer) {
                SetupLocalPlayerComponents(unit, deviceId);
            }
        }

        private void SetupLocalPlayerComponents(Unit character, int deviceId) {
            var playerController = character.GetComponent<PlayerController>();
            playerController.Id = deviceId;
        }
    }
}