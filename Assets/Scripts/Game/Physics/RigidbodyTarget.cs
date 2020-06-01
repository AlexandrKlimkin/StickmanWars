using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTarget : MonoBehaviour {
    public Transform Target;
    private Rigidbody2D _RB;

    private void Awake() {
        _RB = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate() {
        _RB.MovePosition(Target.transform.position);
    }
}
