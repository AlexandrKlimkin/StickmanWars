using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Health;
using Game.Match;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class TrainGenerator : MonoBehaviour {
        [Dependency]
        private readonly SignalBus _SignalBus;

        public GameObject Train;
        public Vector2 TrainLengthVector;
        public VanMoveController MainVan;
        public VanQueue[] VanQueues;
        public Vector2 SpaceBetweenVansVector;
        public VanMoveParameters Parameters;
        public int MaxVansAtTime = 8;
        public float TrainDelay;

        private List<VanMoveController> _ActiveVans = new List<VanMoveController>();

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<MatchStartSignal>(OnMatchStart, this);
        }

        private void OnMatchStart(MatchStartSignal signal) {
            StartCoroutine(GenerateTrainRoutine());
        }

        private IEnumerator GenerateTrainRoutine() {
            yield return new WaitForSeconds(TrainDelay);
            _SignalBus.FireSignal(new TrainMovementSignal(true));
            var vansCount = UnityEngine.Random.Range(TrainLengthVector.x, TrainLengthVector.y);
            var vanXPos = 0f;
            var lastVanWidth = 0f;
            VanMoveController lastVan = null;
            var vanIndex = 0;
            VanQueue vanQueue = null;
            var vanQueueCount = 0;
            var queueVans = 0;
            while (vanIndex < vansCount) {
                var activeVansCount = _ActiveVans.Count;
                if (activeVansCount >= MaxVansAtTime) {
                    yield return null;
                }
                else {
                    var availableVans = VanQueues.Where(_ => _.VanStartIndex <= vanIndex);
                    VanMoveController vanPrefab = null;
                    if(vanIndex > 0) {
                        if(vanQueue == null) {
                            var availableQueues = VanQueues.Where(_ => _.VanStartIndex <= vanIndex).ToList();
                            var index = 0;
                            var availableCount = availableQueues.Count();
                            if (availableCount > 0) {
                                index = UnityEngine.Random.Range(0, availableCount);
                                vanQueue = availableQueues[index];
                                vanQueueCount = UnityEngine.Random.Range(vanQueue.QueueCount.x, vanQueue.QueueCount.y);
                            } else {
                                vanQueue = VanQueues[0];
                            }
                        }
                        vanPrefab = vanQueue.VanMoveController;
                        queueVans++;
                        if (queueVans >= vanQueueCount) {
                            vanQueue = null;
                            queueVans = 0;
                            vanQueueCount = 0;
                        }
                    } else {
                        vanPrefab = MainVan;
                    }
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
                        localPos = lastVan.transform.localPosition -
                                   new Vector3(
                                       lastVanWidth / 2 + vanWidthAdd / 2 + space / Train.transform.localScale.x, 0, 0);
                    }
                    lastVanWidth = vanWidth;
                    van.transform.localPosition = localPos;
                    van.SetParameters(Parameters);
                    _ActiveVans.Add(van);
                    van.SimpleDamageable.OnKill += OnVanKill;
                    vanIndex++;
                    lastVan = van;
                    var boxGenerators = van.GetComponentsInChildren<VanBoxesGenerator>();
                    boxGenerators.ForEach(_=>_?.GenerateBoxes(van.Rigidbody));
                    var objectsGenerator = van.GetComponentInChildren<VanObjectsGenerator>();
                    objectsGenerator?.Generate(van.Rigidbody);
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
