using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Health;
using Game.Match;
using KlimLib.SignalBus;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class RespawnModeService : ILoadableService, IUnloadableService, IPlayerLifesCounter {
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
        private readonly UnityEventProvider _EventProvider;
        [Dependency]
        private readonly GameManagerService _GameManagerService;

        private Dictionary<byte, int> _PlayersLifesDict;

        private int PlayersAlive => AlivePlayers.Count;

        private List<byte> AlivePlayers => _PlayersLifesDict.Where(_ => _.Value > 0).Select(_=>_.Key).ToList();

        public IReadOnlyDictionary<byte, int> PlayersLifesDict => _PlayersLifesDict;

        public const int PlayerLifes = 5;
        private const float _RespawnDelay = 2f;


        public void Load() {
            ContainerHolder.Container.RegisterInstance<IPlayerLifesCounter>(this);
            _SignalBus.Subscribe<MatchStartSignal>(OnMatchStart, this);
            //_SignalBus.Subscribe<PlayerAddedSignal>(OnPlayerAdded, this);
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeath, this);
            _PlayersConnectionService.AddBots();
            InitializeNewMatch();
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void InitializeNewMatch() {
            _PlayersLifesDict = new Dictionary<byte, int>();
            foreach (var player in _MatchData.Players) {
                _PlayersLifesDict.Add(player.PlayerId, PlayerLifes);
            }
        }

        private void OnMatchStart(MatchStartSignal signal) {
            _EventProvider.StartCoroutine(SpawnAllAtTheBeginingRoutine()); //ToDo: Fix this shit
        }

        //private void OnPlayerAdded(PlayerAddedSignal signal) {
        //    _PlayersLifesDict.Add(signal.PlayerData.PlayerId, PlayerLifes);
        //    if (signal.SpawnOnMap)
        //        SpawnCharacterInRandomPos(signal.PlayerData.PlayerId);
        //}

        private IEnumerator SpawnAllAtTheBeginingRoutine() {
            yield return new WaitForEndOfFrame(); 
            SpawnAllAtTheBegining();
        }

        private void SpawnAllAtTheBegining() {
            var availablePoints = _PlayersSpawnSettings.PlayerSpawnPoints.ToList();
            foreach (var player in _MatchData.Players) {
                var randomPointIndex = Random.Range(0, availablePoints.Count);
                var point = availablePoints[randomPointIndex];
                availablePoints.Remove(point);
                SpawnPlayerCharacter(player, point.Point.position); 
            }
        }

        private void SpawnPlayerCharacter(PlayerData playerData, Vector3 position) {
            var deviceIndex = _PlayersConnectionService.GetDeviceIndex(playerData.PlayerId).Value;
            _CharacterCreationService.CreateCharacter(playerData, true, deviceIndex, position);
        }

        private void SpawnPlayerCharacter(byte playerId, Vector3 position) {
            var playerData = _MatchService.GetPlayerData(playerId);
            SpawnPlayerCharacter(playerData, position);
        }

        private void OnCharacterDeath(CharacterDeathSignal signal) {
            var dmg = signal.Damage;
            var playerId = dmg.Receiver.OwnerId.Value;
            _PlayersLifesDict[playerId]--;
            if (_PlayersLifesDict[playerId] > 0) {
                _EventProvider.StartCoroutine(RespawnRoutine(playerId));
            }
            else {
                PlayerDefeated(playerId);
            }
        }

        private IEnumerator RespawnRoutine(byte playerId) {
            yield return new WaitForSeconds(_RespawnDelay);
            SpawnCharacterInRandomPos(playerId);
        }

        private void SpawnCharacterInRandomPos(byte playerId) {
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
            _GameManagerService.EndMatch();
            Debug.Log(PlayersAlive > 0 ? $"Match end. Player {AlivePlayers.First()} win!" : $"Match end.");
        }

        private int GetRandomRespawnPointIndex() {
            return Random.Range(0, _PlayersSpawnSettings.PlayerRespawnPoints.Count);
        }
    }
}