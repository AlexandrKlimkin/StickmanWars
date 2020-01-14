using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKMuscle : MonoBehaviour
{
    public Transform Target;
    public float Force;
    public bool Enabled;

    private Rigidbody2D _Rigidbody;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ActivateMuscle();
    }

    private void ActivateMuscle()
    {
        if (!Enabled)
            return;
        RotateSmooth(Target.rotation.eulerAngles.z, Force);
    }

    private void RotateSmooth(float rotation, float force) {
        var angle = Mathf.DeltaAngle(_Rigidbody.rotation, rotation);
        var ratio = angle / 180;
        ratio *= ratio;
        _Rigidbody.MoveRotation(Mathf.LerpAngle(_Rigidbody.rotation, rotation, force * ratio * Time.fixedDeltaTime));
        _Rigidbody.AddTorque(angle * force * (1 - ratio) * .1f);
    }
}
