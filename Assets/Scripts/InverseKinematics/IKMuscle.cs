using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
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

    private void FixedUpdate()
    {
        ActivateMuscle();
    }

    private void ActivateMuscle()
    {
        if (!Enabled)
            return;
        //RotateSmooth(Target.rotation.eulerAngles.z, Force);
        Pin();
    }

    private void RotateSmooth(float rotation, float force) {
        var angle = Mathf.DeltaAngle(_Rigidbody.rotation, rotation);
        var ratio = angle / 180;
        ratio *= ratio;
        _Rigidbody.MoveRotation(Mathf.LerpAngle(_Rigidbody.rotation, rotation, force * ratio * Time.fixedDeltaTime));
        _Rigidbody.AddTorque(angle * force * (1 - ratio) * .1f);
    }

    private void Pin()
    {
        var tam = MathExtensions.TransformPointUnscaled(Target, _Rigidbody.centerOfMass);
        var posOffset = (Vector2)tam - _Rigidbody.worldCenterOfMass;
        posOffset /= Time.fixedDeltaTime;
        _Rigidbody.velocity = posOffset;
    }
}
