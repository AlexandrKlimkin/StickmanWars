using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityAttacher : MonoBehaviour {

    private Rigidbody2D _Rigidbody2D;

    private void Awake() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider == null)
            return;
        if (collision.rigidbody == null)
            return;
        var velInheritor = collision.gameObject.GetComponent<IVelocityInheritor>();
        if (velInheritor == null)
            return;
        velInheritor.AttachedToRB = _Rigidbody2D;
        velInheritor.CanDetach = false;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider == null)
            return;
        if (collision.rigidbody == null)
            return;
        var velInheritor = collision.gameObject.GetComponent<IVelocityInheritor>();
        if (velInheritor == null)
            return;
        velInheritor.CanDetach = true;
    }

    private void OnDestroy() {
        
    }
}
