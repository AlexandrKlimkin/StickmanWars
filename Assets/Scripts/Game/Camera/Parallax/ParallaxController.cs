using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public Transform Target;

    public List<ParallaxObject> ParallaxObjects;
    private Vector3 _LastTargetPosition;

    //private void Awake()
    //{
    //    GetComponentsInChildren(ParallaxObjects);
    //}

    private void Start()
    {
        _LastTargetPosition = Target.position;
    }

    private void Update()
    {
        if(ParallaxObjects == null)
            return;
        var targetSpeed = Target.transform.position - _LastTargetPosition;
        foreach (var obj in ParallaxObjects)
        {
            var velocity = new Vector3(targetSpeed.x * obj.SpeedX, targetSpeed.y * obj.SpeedY, 0);
            obj.transform.position += velocity;
        }
        _LastTargetPosition = Target.position;
    }
}
