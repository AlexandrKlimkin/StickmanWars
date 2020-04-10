using System.Collections;
using System.Collections.Generic;
using Game.CameraTools;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

public class ParallaxController : MonoBehaviour {
    [Dependency]
    private readonly SignalBus _SignalBus;

    public List<ParallaxObject> ParallaxObjects;
    private Vector3 _LastTargetPosition;
    private float _LastZoom;

    private GameCameraBehaviour _Camera;

    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        _SignalBus.Subscribe<GameCameraSpawnedSignal>(OnGameCameraSpawn, this);
    }

    private void Update()
    {
        if(_Camera == null)
            return;
        if(ParallaxObjects == null)
            return;
        var targetSpeed = _Camera.transform.position - _LastTargetPosition;
        foreach (var obj in ParallaxObjects)
        {
            var velocity = new Vector3(targetSpeed.x * obj.SpeedX, targetSpeed.y * obj.SpeedY, 0);
            obj.transform.position += velocity;
            var zoomChange = _Camera.Zoom - _LastZoom;
            var newScale = zoomChange * obj.ScaleMult;
            obj.transform.localScale -= new Vector3(newScale, newScale, newScale);
        }
        _LastTargetPosition = _Camera.transform.position;
        _LastZoom = _Camera.Zoom;
    }

    private void OnGameCameraSpawn(GameCameraSpawnedSignal signal) {
        _Camera = signal.Camera;
        _LastTargetPosition = _Camera.transform.position;
        _LastZoom = _Camera.Zoom;
    }
}
