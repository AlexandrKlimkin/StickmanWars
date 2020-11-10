using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Health;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class TrainGenerator : MonoBehaviour {
        public GameObject Train;
        public Vector2 TrainLengthVector;
        public VanMoveController MainVan;
        public VanMoveController[] VanPrefabs;
        public Vector2 SpaceBetweenVansVector;
        public VanMoveParameters Parameters;
        public int MaxVansAtTime = 8;

        private List<VanMoveController> _ActiveVans = new List<VanMoveController>();

        private void Start() {
            StartCoroutine(GenerateTrainRoutine());
        }

        private IEnumerator GenerateTrainRoutine() {
            var vansCount = UnityEngine.Random.Range(TrainLengthVector.x, TrainLengthVector.y);

            var vanXPos = 0f;
            var lastVanWidth = 0f;
            VanMoveController lastVan = null;
            var vanIndex = 0;
            while (vanIndex < vansCount) {
                var activeVansCount = _ActiveVans.Count;
                if (activeVansCount >= MaxVansAtTime) {
                    yield return null;
                }
                else {
                    var vanPrefab = vanIndex > 0 ? VanPrefabs[UnityEngine.Random.Range(0, VanPrefabs.Length)] : MainVan;
                    var van = Instantiate(vanPrefab, Train.transform);
                    var vanCollider = van.GetComponentInChildren<Collider2D>();
                    var vanWidth = vanCollider.bounds.size.x;
                    var vanWidthAdd = 0f;
                    var space = 0f;
                    if (vanIndex > 0) {
                        vanWidthAdd = vanWidth;
                        space = UnityEngine.Random.Range(SpaceBetweenVansVector.x, SpaceBetweenVansVector.y);
                    }
                    Vector3 localPos;
                    if (lastVan == null)
                        localPos = Vector3.zero;
                    else {
                        vanXPos -= (lastVanWidth / 2 + vanWidthAdd / 2 + space) / Train.transform.localScale.x;
                        lastVanWidth = vanWidth;
                        localPos = lastVan.transform.localPosition -
                                   new Vector3(
                                       lastVanWidth / 2 + vanWidthAdd / 2 + space / Train.transform.localScale.x, 0, 0);
                    }
                    van.transform.localPosition = localPos;
                    van.SetParameters(Parameters);
                    _ActiveVans.Add(van);
                    if (lastVan != null && lastVan.Moving) {
                        van.SetMoving(lastVan.Rigidbody.velocity, lastVan.Timer);
                    }
                    van.SimpleDamageable.OnKill += OnVanKill;
                    vanIndex++;
                    lastVan = van;
                    var boxGenerator = van.GetComponentInChildren<VanBoxesGenerator>();
                    boxGenerator?.GenerateBoxes();
                    var objectsGenerator = van.GetComponentInChildren<VanObjectsGenerator>();
                    objectsGenerator?.Generate();
                }
            }
        }

        private void OnVanKill(SimpleDamageable dmgbl, Damage dmg) {
            var van = _ActiveVans.FirstOrDefault(_ => _.SimpleDamageable == dmgbl);
            if(van == null || !_ActiveVans.Contains(van))
                return;
            _ActiveVans.Remove(van);
            if(van.SimpleDamageable == null)
                return;
            van.SimpleDamageable.OnKill -= OnVanKill;
        }

        private void OnDestroy() {
            foreach (var van in _ActiveVans) {
                if(van == null)
                    return;
                if(van.SimpleDamageable == null)
                    return;
                van.SimpleDamageable.OnKill -= OnVanKill;
            }
        }
    }
}
