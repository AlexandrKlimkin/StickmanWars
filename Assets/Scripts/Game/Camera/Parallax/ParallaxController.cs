using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Game.CameraTools;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

public class ParallaxController : MonoBehaviour {
    [Dependency]
    private readonly SignalBus _SignalBus;

    public List<ParallaxObject> ParallaxObjects;
    public List<DiagonalParallaxObject> DiagonalParallaxObjects;

    private Vector3 _LastTargetPosition;
    private float _LastZoom;

    private ProCamera2D _Camera;

    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        _SignalBus.Subscribe<GameCameraSpawnedSignal>(OnGameCameraSpawn, this);
    }

    private void Update()
    {
        if(_Camera == null)
            return;
        var targetSpeed = _Camera.transform.position - _LastTargetPosition;
        if (ParallaxObjects != null) {
            foreach (var obj in ParallaxObjects) {
                var velocity = new Vector3(targetSpeed.x * obj.SpeedX, targetSpeed.y * obj.SpeedY, 0);
                obj.transform.position += velocity;
            }
        }
        if(DiagonalParallaxObjects != null) {
            foreach (var obj in DiagonalParallaxObjects) {
                var rotateY = targetSpeed.x * obj.RotY;
                var rotateZ = targetSpeed.y * obj.RotZ;
                var eulerAngles = obj.transform.rotation.eulerAngles;
                obj.transform.rotation = Quaternion.Euler(eulerAngles.x + 0, eulerAngles.y + rotateY, eulerAngles.z + rotateZ);
            }
        }
        _LastTargetPosition = _Camera.transform.position;
    }

    private void OnGameCameraSpawn(GameCameraSpawnedSignal signal) {
        _Camera = signal.Camera;
        _LastTargetPosition = _Camera.transform.position;
        //_LastZoom = _Camera.Zoom;
    }
}
