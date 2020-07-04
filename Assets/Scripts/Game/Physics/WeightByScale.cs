using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightByScale : MonoBehaviour {
    private void Awake() {
        GetComponent<Rigidbody2D>().mass *= transform.lossyScale.x * transform.lossyScale.y;
    }
}
