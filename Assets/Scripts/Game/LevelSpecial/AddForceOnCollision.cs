using Assets.Scripts.Tools;
using Character.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOnCollision : MonoBehaviour
{
    public LayerMask LayerMask;
    public Transform ForceStartPoint;
    public float ForceMagnitude;
    public float Damage;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (LayerMask == (LayerMask | (1 << collider.gameObject.layer))) {
            var force = (collider.attachedRigidbody.position - ForceStartPoint.position.ToVector2()).normalized;
            collider.attachedRigidbody.AddForce(force * ForceMagnitude);
            var damageable = collider.attachedRigidbody.GetComponent<IDamageable>();
            damageable.ApplyDamage(new Damage(null, damageable, Damage));
        }
    }
}
