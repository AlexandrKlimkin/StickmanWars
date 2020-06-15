using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Health;
using Game.Match;
using KlimLib.SignalBus;
using Tools.Services;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class RespawnModeService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly PlayersSpawnSettings _PlayersSpawnSettings;
        [Dependency]
        private readonly CharacterCreationService _CharacterCreationService;
        [Dependency]
        private readonly MatchData _MatchData;
        [Dependency]
        private readonly PlayersConnectionService _PlayersConnectionService;
        [Dependency]
        private readonly MatchService _MatchService;
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly GameManagerService _GameManager;
        [Dependency]
        private readonly UnityEventProvider _EventProvider;

        private Dictionary<byte, int> _PlayersLifesDict;

        private int PlayersAlive => AlivePlayers.Count;

        private List<byte> AlivePlayers => _PlayersLifesDict.Where(_ => _.Value > 0).Select(_=>_.Key).ToList();

        public const int PlayerLifes = 3;
        private const float _RespawnDelay = 2f;


        public void Load() {
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeath, this);
            InitializeNewMatch();
            SpawnAllAtTheBegining();
        }

        public void Unload() {
            _SignalBus.UnSubscribe<CharacterDeathSignal>(this);
        }


        public void InitializeNewMatch() {
            _PlayersLifesDict = new Dictionary<byte, int>();
            foreach (var player in _MatchData.Players) {
                _PlayersLifesDict.Add(player.PlayerId, PlayerLifes);
            }
        }

        private void SpawnAllAtTheBegining() {
            var availablePoints = _PlayersSpawnSettings.PlayerSpawnPoints.ToList();
            foreach (var player in _MatchData.Players) {
                var randomPointIndex = Random.Range(0, availablePoints.Count);
                var point = availablePoints[randomPointIndex];
                availablePoints.Remove(point);
                _CharacterCreationService.CreateCharacter(player.CharacterId, player.PlayerId, true, _PlayersConnectionService.GetDeviceIndex(player.PlayerId).Value, point.Point.position);
            }
        }

        private void SpawnPlayerCharacter(PlayerData playerData, Vector3 position) {
            var deviceIndex = _PlayersConnectionService.GetDeviceIndex(playerData.PlayerId).Value;
            _CharacterCreationService.CreateCharacter(playerData.CharacterId, playerData.PlayerId, true, deviceIndex, position);
        }

        private void SpawnPlayerCharacter(byte playerId, Vector3 position) {
            var playerData = _MatchService.GetPlayerData(playerId);
            SpawnPlayerCharacter(playerData, position);
        }

        private void OnCharacterDeath(CharacterDeathSignal signal) {
            _PlayersLifesDict[signal.PlayerId]--;
            if (_PlayersLifesDict[signal.PlayerId] > 0) {
                _EventProvider.StartCoroutine(RespawnRoutine(signal.PlayerId));
            }
            else {
                PlayerDefeated(signal.PlayerId);
            }
        }

        private IEnumerator RespawnRoutine(byte playerId) {
            yield return new WaitForSeconds(_RespawnDelay);
            var respawnPoint = GetRandomRespawnPointIndex();
            var pos = _PlayersSpawnSettings.PlayerRespawnPoints[respawnPoint].Point.position;
            SpawnPlayerCharacter(playerId, pos);
        }

        private void PlayerDefeated(byte playerId) {
            Debug.Log($"Player {playerId} defeated.");
            if (PlayersAlive <= 1) {
                EndMatch();
            }
        }

        private void EndMatch() {
            _SignalBus.FireSignal(new MatchEndSignal());
            _GameManager.EndGame();
            Debug.Log(PlayersAlive > 0 ? $"Match end. Player {AlivePlayers.First()} win!" : $"Match end.");
        }

        private int GetRandomRespawnPointIndex() {
            return Random.Range(0, _PlayersSpawnSettings.PlayerRespawnPoints.Count);
        }
    }
}