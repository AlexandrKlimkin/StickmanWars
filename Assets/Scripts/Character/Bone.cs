using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public MuscleType Type;
    public Transform UpAxis;
    public Transform DownAxis;
    public Transform MidleAxis;
    public List<SpriteRenderer> SpriteRenderers;
    public CapsuleCollider2D CapsuleCollider;

    private void Awake()
    {
        GetComponents(SpriteRenderers);
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }
}