using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour {
    public Vector2 Velocity;
    public float Acceleration;
    public float Delay;

    private Rigidbody2D _Rigidbody;

    void Awake() {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(DelayRoutine());
    }

    private IEnumerator DelayRoutine() {
        yield return new WaitForSeconds(Delay);
        while (_Rigidbody.velocity.x < Velocity.x) {
            _Rigidbody.velocity += Velocity.normalized * Acceleration * Time.deltaTime;
            yield return null;
        }
        _Rigidbody.velocity = Vector2.ClampMagnitude(_Rigidbody.velocity, Velocity.magnitude);
    }
}
