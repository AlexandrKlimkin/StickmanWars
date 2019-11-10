using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCollider : MonoBehaviour
{
    public event Action GroundCollisionEnter;
    public event Action GroundCollisionStay;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "ground") {
            GroundCollisionEnter?.Invoke();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "ground") {
            GroundCollisionStay?.Invoke();
        }
    }
}
