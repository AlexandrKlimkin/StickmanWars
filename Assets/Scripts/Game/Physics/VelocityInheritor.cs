using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityInheritor : MonoBehaviour {
    private List<Rigidbody2D> _StayedObjects;
    public Rigidbody2D Rigidbody;
    private Vector2 _LastPos;

    private void Awake() {
        _StayedObjects = new List<Rigidbody2D>();
    }

    private void Start() {
        _LastPos = Rigidbody.position;
    }

    private void FixedUpdate() {
        var objToRemove = new List<Rigidbody2D>();
        var rbDelta = Rigidbody.position - _LastPos;
        _StayedObjects.ForEach(_ => {
            if (_)
                _.position = _.position + rbDelta;
            else
                objToRemove.Add(_);
        });
        objToRemove.ForEach(_ => RemoveObject(_));
        _LastPos = Rigidbody.position;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        AddObject(col.attachedRigidbody);
    }

    private void OnTriggerExit2D(Collider2D col) {
        RemoveObject(col.attachedRigidbody);
    }

    public void AddObject(Rigidbody2D rb) {
        if (rb == null)
            return;
        if (_StayedObjects.Contains(rb))
            return;
        _StayedObjects.Add(rb);
    }

    public void RemoveObject(Rigidbody2D rb) {
        if (_StayedObjects.Contains(rb))
            _StayedObjects.Remove(rb);
    }
}
