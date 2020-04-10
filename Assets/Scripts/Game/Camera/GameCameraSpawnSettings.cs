using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

public class GameCameraSpawnSettings : MonoBehaviour {

    public Transform SpawnTransform;

    private void Awake() {
        ContainerHolder.Container.RegisterInstance(this);
    }

    private void OnDestroy() {
        ContainerHolder.Container.Unregister<GameCameraSpawnSettings>();
    }
}
