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
    private List<GameObject> _PartPrefabs1;
    [SerializeField]
    private Vector2Int _RandomCountPrefabs1;

    [SerializeField]
    private List<GameObject> _PartPrefabs2;
    [SerializeField]
    private Vector2Int _RandomCountPrefabs2;

    [SerializeField]
    private List<GameObject> _PartPrefabs3;
    [SerializeField]
    private Vector2Int _RandomCountPrefabs3;


    [SerializeField]
    private Vector2 _RandExplosionForceVector;
    [SerializeField]
    private bool _UseParentForse = true;
    [SerializeField]
    private float _AverageDmg = 10f;

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
        if (_PartPrefabs1.Count > 0)
            SpawnPart(damage, _PartPrefabs1, _RandomCountPrefabs1);
        if(_PartPrefabs2.Count > 0)
            SpawnPart(damage, _PartPrefabs2, _RandomCountPrefabs2);
        if (_PartPrefabs3.Count > 0)
            SpawnPart(damage, _PartPrefabs3, _RandomCountPrefabs3);
        Destroy(gameObject);
    }

    private void SpawnPart(Damage damage, List<GameObject> partList, Vector2Int rand) {
        var count = Random.Range(rand.x, rand.y + 1);
        var availablepartList = partList.ToList();
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
            AddExplosionPartsForces(parts, damage);
        }
    }

    private void AddExplosionPartsForces(List<GameObject> parts, Damage dmg) {
        foreach (var part in parts) {
            var partRB = part.GetComponent<Rigidbody2D>();
            //if (_UseParentForse) {
            //    partRB.velocity = Rigidbody.velocity;
            //}
            //if (partRB != null) {
            if (_RandExplosionForceVector != Vector2.zero) {
                var randVector = _RandExplosionForceVector * dmg.Amount / _AverageDmg;
                var randomMagnitude = Random.Range(_RandExplosionForceVector.x, _RandExplosionForceVector.y);
                var randomDir = Random.onUnitSphere;
                var finalForce = randomDir * randomMagnitude;
                partRB.AddForce(finalForce);
            }
        }
    }
}