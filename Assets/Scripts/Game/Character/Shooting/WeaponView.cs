using Game.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    public Transform ShootTransform;
    public GameObject CollidersContainer;

    private List<Collider2D> _Colliders = new List<Collider2D>();
    private float _StartXScaleSign;

    public Rigidbody2D Rigidbody { get; private set; }
    public Levitation Levitation { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Levitation = GetComponent<Levitation>();
        CollidersContainer.GetComponentsInChildren(_Colliders);
    }

    private void Start()
    {
        _StartXScaleSign = Mathf.Sign(transform.lossyScale.x);
    }

    public void PickUp(Transform place)
    {
        Rigidbody.simulated = false;
        CollidersContainer.SetActive(false);
        transform.SetParent(place);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * _StartXScaleSign, transform.localScale.y, transform.localScale.z);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        StopAllCoroutines();
    }

    public void ThrowOut(GameObject thrower)
    {
        Rigidbody.simulated = true;
        CollidersContainer.SetActive(true);
        transform.SetParent(null);
        StopAllCoroutines();
        StartCoroutine(IgnorThrowerCollisionRoutine(thrower));
    }

    private IEnumerator IgnorThrowerCollisionRoutine(GameObject thrower)
    {
        var throwerColliders = thrower.GetComponentsInChildren<Collider2D>();
        foreach (var col in _Colliders)
        {
            foreach (var throwerCol in throwerColliders) {
                Physics2D.IgnoreCollision(throwerCol, col);
            }
        }
        yield return new WaitForSeconds(1f);
        foreach (var col in _Colliders) {
            foreach (var throwerCol in throwerColliders) {
                Physics2D.IgnoreCollision(throwerCol, col, false);
            }
        }
    }
}
