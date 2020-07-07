using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyVelocityLimitsLog : MonoBehaviour {
    private float _MaxVelocityMagnitude;

    private Rigidbody2D _Rigidbody;

    private void Start() {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        var magnitudeVel = _Rigidbody.velocity.magnitude;
        if (magnitudeVel > _MaxVelocityMagnitude) {
            _MaxVelocityMagnitude = magnitudeVel;
            Debug.Log($"{gameObject.name} max velocity is {_MaxVelocityMagnitude}");
        }
    }
}
