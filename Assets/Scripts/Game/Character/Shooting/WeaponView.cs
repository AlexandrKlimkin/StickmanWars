using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    public Transform ShootTransform;
    public GameObject CollidersContainer;

    private List<Collider2D> _Colliders = new List<Collider2D>();
    private Vector3 _StartlocalScale;

    public Rigidbody2D Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        CollidersContainer.GetComponentsInChildren(_Colliders);
    }

    private void Start()
    {
        _StartlocalScale = transform.localScale;
    }

    public void PickUp(Transform place)
    {
        Rigidbody.simulated = false;
        CollidersContainer.SetActive(false);
        transform.SetParent(place);
        transform.localScale = _StartlocalScale;
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
