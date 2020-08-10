using System.Collections;
using System.Collections.Generic;
using Game.CameraTools;
using UnityDI;
using UnityEngine;

public class GameCameraSettings : MonoBehaviour {

    public Transform SpawnTransform;

    private void Awake() {
        ContainerHolder.Container.RegisterInstance(this);
    }

    private void OnDestroy() {
        ContainerHolder.Container.Unregister<GameCameraSettings>();
    }
}
