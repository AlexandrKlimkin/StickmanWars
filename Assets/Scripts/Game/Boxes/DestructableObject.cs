using Character.Health;
using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityDI;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IDamageable {
    [Dependency]
    private readonly SignalBus _SignalBus;

    [SerializeField]
    private float _MaxHealth;
    [SerializeField]
    private List<GameObject> _PartPrefabs;
    [SerializeField]
    private Vector2Int _RandomCount;
    [SerializeField]
    private Vector2 _RandExplosionForceVector;
    [SerializeField]
    private bool _UseParentForse = true;

    public byte? OwnerId => 255;

    public float Health { get; set; }
    public float MaxHealth => _MaxHealth;
    public float NormilizedHealth => Health / _MaxHealth;
    public Collider2D Collider { get; set; }
    public Rigidbody2D Rigidbody { get; set; }

    public bool Dead { get; set; }

    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        Collider = GetComponent<Collider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Health = MaxHealth;
    }

    public void ApplyDamage(Damage damage) {
        _SignalBus.FireSignal(new ApplyDamageSignal(damage));
    }

    public void Kill(Damage damage) {
        var count = Random.Range(_RandomCount.x, _RandomCount.y);
        var availablepartList = _PartPrefabs.ToList();
        var parts = new List<GameObject>();
        for (int i = 0; i < count; i++) {
            var index = Random.Range(0, availablepartList.Count);
            var part = availablepartList[index];
            availablepartList.Remove(part);
            var newPart = Instantiate(part, transform.parent);
            newPart.transform.localPosition = transform.localPosition;
            newPart.transform.localRotation = transform.localRotation;
            newPart.transform.localScale = transform.localScale;
            parts.Add(newPart);
        }
        if (Rigidbody != null) {
            AddExplosionPartsForces(parts);
        }
        Destroy(gameObject);
    }

    private void AddExplosionPartsForces(List<GameObject> parts) {
        foreach (var part in parts) {
            var partRB = part.GetComponent<Rigidbody2D>();
            //if (_UseParentForse) {
            //    partRB.velocity = Rigidbody.velocity;
            //}
            //if (partRB != null) {
            if (_RandExplosionForceVector != Vector2.zero) {
                var randomMagnitude = Random.Range(_RandExplosionForceVector.x, _RandExplosionForceVector.y);
                var randomDir = Random.onUnitSphere;
                var finalForce = randomDir * randomMagnitude;
                partRB.AddForce(finalForce);
            }
        }
    }
}