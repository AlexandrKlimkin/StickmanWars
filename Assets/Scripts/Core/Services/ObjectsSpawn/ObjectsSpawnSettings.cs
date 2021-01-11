using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityDI;
using UnityEngine;

namespace Core.Services.Game {
    public class ObjectsSpawnSettings : MonoBehaviour {

        public Vector2 RandomDelay;
        public int MaxCount;
        public List<ObjectSpawnData> ObjectsSpawnData;
        public List<Transform> SpawnPoints;

        private void Awake() {
            ContainerHolder.Container.RegisterInstance(this);
        }

        private void OnDestroy() {
            ContainerHolder.Container.Unregister<ObjectsSpawnSettings>();
        }
    }

    [Serializable]
    public class ObjectSpawnData {
        public GameObject Prefab;
        public int MaxAmount;
    }
}