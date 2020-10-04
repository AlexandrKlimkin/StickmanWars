using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour {
    public Vector2 Velocity;

    private Rigidbody2D _Rigidbody;

    void Awake() {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        _Rigidbody.velocity = Velocity;
    }
}
