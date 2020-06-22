using System.Collections;
using Game.Match;
using KlimLib.SignalBus;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class ObjectsSpawnService : ILoadableService, IUnloadableService {
        [Dependency]
        private readonly UnityEventProvider _EventProvider;
        [Dependency]
        private readonly ObjectsSpawnSettings _Settings;
        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly GameManagerService _GameManagerService;



        public void Load() {
            _SignalBus.Subscribe<MatchEndSignal>(OnMatchEnd, this);
            StartSpawn();
        }

        private IEnumerator ObjectsSpawnRoutine() {
            while (true) {
                if(!_GameManagerService.GameInProgress)
                    yield return null;
                if (WeaponsInfoContainer.AllWeapons.Count < _Settings.MaxCount) {
                    var randomIndex = Random.Range(0, _Settings.ObjectsSpawnData.Count);
                    var randomObjectData = _Settings.ObjectsSpawnData[randomIndex];
                    var randomPointIndex = Random.Range(0, _Settings.SpawnPoints.Count);
                    var randomPoint = _Settings.SpawnPoints[randomPointIndex];
                    Object.Instantiate(randomObjectData.Prefab, randomPoint.position, randomPoint.rotation);
                    var randomDelay = Random.Range(_Settings.RandomDelay.x, _Settings.RandomDelay.y);
                    yield return new WaitForSeconds(randomDelay);
                } else
                    yield return null;
            }
        }

        private void StartSpawn() {
            _EventProvider.StartCoroutine(ObjectsSpawnRoutine());
        }

        private void StopSpawn() {
            _EventProvider.StopCoroutine(ObjectsSpawnRoutine());
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
            StopSpawn();
        }

        private void OnMatchEnd(MatchEndSignal signal) {
            StopSpawn();
        }
    }
}
