using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Character.MuscleSystem;
using UnityEngine;

public class HitApply : MonoBehaviour, IDamageable
{
    public Collider2D Collider { get; set; }
    public Transform Applyer;

    public void ApplyDamage(Damage damage)
    {

    }

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }
}
