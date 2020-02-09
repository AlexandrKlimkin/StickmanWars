using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetMaster : MonoBehaviour
{
    public Rigidbody2D RB;
    public Vector2 Force;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RB?.AddForce(Force);
        }
    }
}
