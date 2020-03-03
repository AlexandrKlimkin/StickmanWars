using System.Collections;
using System.Collections.Generic;
using Rendering;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public List<ParallaxObject> ParallaxObjects;
    private Vector3 _LastTargetPosition;
    private float _LastZoom;

    private GameCameraBehaviour _Camera;

    private void Start()
    {
        _Camera = GameCameraBehaviour.Instance;
        _LastTargetPosition = _Camera.transform.position;
        _LastZoom = _Camera.Zoom;
    }

    private void Update()
    {
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
}
