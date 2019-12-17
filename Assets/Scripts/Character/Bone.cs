using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public Transform UpAxis;
    public Transform DownAxis;
    public Transform MidleAxis;
    public SpriteRenderer SpriteRenderer;
    public CapsuleCollider2D CapsuleCollider;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }
}