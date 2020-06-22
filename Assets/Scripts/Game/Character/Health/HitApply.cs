using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Character.MuscleSystem;
using UnityEngine;

public class HitApply : MonoBehaviour, IDamageable
{
    public Collider2D Collider { get; set; }

    public float Health => 0;

    public float NormilizedHealth => 1;

    public byte? OwnerId => null;

    public Transform Applyer;

    public void ApplyDamage(Damage damage)
    {

    }

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }

    public void Kill() {
        throw new System.NotImplementedException();
    }
}
