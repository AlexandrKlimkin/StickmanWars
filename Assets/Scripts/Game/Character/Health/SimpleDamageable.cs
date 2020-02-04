using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class SimpleDamageable : MonoBehaviour, IDamageable
{
    public Collider2D Collider { get; set; }

    public void ApplyDamage(Damage damage)
    {

    }

    private void Awake()
    {
        Collider = gameObject.GetComponent<Collider2D>();

    }
}
