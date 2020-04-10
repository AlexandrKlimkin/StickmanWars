using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour {
    public float xRotSpeed;
    public float yRotSpeed;
    public float zRotSpeed;

    private void Update() {
        transform.Rotate(xRotSpeed * Time.deltaTime, yRotSpeed * Time.deltaTime, zRotSpeed * Time.deltaTime);
    }
}
