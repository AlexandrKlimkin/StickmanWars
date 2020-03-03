using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [Range(0,1f)]
    public float SpeedX;
    [Range(0, 1f)]
    public float SpeedY;
    [Range(0, 1f)]
    public float ScaleMult;
}
