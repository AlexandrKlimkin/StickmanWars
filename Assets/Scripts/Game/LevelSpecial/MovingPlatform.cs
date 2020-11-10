using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    public LayerMask LayerMask;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (LayerMask == (LayerMask | (1 << collision.gameObject.layer))) {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (LayerMask == (LayerMask | (1 << collision.gameObject.layer))) {
            collision.collider.transform.SetParent(null);
        }
    }
}
