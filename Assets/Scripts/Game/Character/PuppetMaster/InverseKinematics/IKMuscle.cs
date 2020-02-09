using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEngine;

public class IKMuscle : MonoBehaviour
{
    public Transform Target;
    [Range(0,1f)]
    public float Weight;
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
        Pin();
    }

    private void Pin()
    {
        Weight = Mathf.Clamp01(Weight);
        if(Weight == 0)
            return;
        var tam = MathExtensions.TransformPointUnscaled(Target, _Rigidbody.centerOfMass);
        var posOffset = (Vector2)tam - _Rigidbody.worldCenterOfMass;
        posOffset /= Time.fixedDeltaTime;
        _Rigidbody.velocity = posOffset * Weight;
    }
}
