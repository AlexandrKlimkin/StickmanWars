using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraBehaviour : MonoBehaviour
{
    public Transform Target;
    public float Damping;
    public Vector3 Offset;

    void Update()
    {
        var targetPos = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * Damping);
        transform.position = new Vector3(targetPos.x, targetPos.y, -10) + Offset;
    }
}
