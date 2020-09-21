using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyVelocityLimits : MonoBehaviour {
    public float MaxVelocityMagnitude = 250;
    private Rigidbody2D _Rigidbody;

    private void Start() {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        var sqrMag = _Rigidbody.velocity.sqrMagnitude;
        if (sqrMag > MaxVelocityMagnitude * MaxVelocityMagnitude) {
            _Rigidbody.velocity = _Rigidbody.velocity.normalized * MaxVelocityMagnitude;
           // Debug.Log($"{gameObject.name} max velocity is {_Rigidbody.velocity.magnitude}");
        }
    }
}
