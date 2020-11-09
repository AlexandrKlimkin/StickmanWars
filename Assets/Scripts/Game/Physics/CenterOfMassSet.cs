using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMassSet : MonoBehaviour {
    public Rigidbody2D Rigidbody;
    public Transform CenterMassPoint;
    void Start() {
        if (Rigidbody == null)
            return;
        Rigidbody.centerOfMass = CenterMassPoint.localPosition;
    }
}
