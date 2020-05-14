using System;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class PlayersSpawnSettings : MonoBehaviour {

        public List<PlayerSpawnPointData> PlayerSpawnPoints;
        public List<PlayerSpawnPointData> PlayerRespawnPoints;

        private void Awake() {
            ContainerHolder.Container.RegisterInstance(this);
        }

        private void OnDestroy() {
            ContainerHolder.Container.Unregister(GetType());
        }
    }

    [Serializable]
    public class PlayerSpawnPointData {
        public int Id;
        public Transform Point;
    }
}
