using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class TrainGenerator : MonoBehaviour {
        public GameObject TrainTemplate;
        public Vector2 TrainLengthVector;
        public GameObject MainVan;
        public GameObject[] VanPrefabs;
        public Vector2 SpaceBetweenVansVector;

        private void Start() {
            GenerateTrain();
        }

        private void GenerateTrain() {
            var trainobj = Instantiate(TrainTemplate, transform);
            trainobj.transform.localPosition = Vector3.zero;
            trainobj.transform.localRotation = Quaternion.identity;

            var vansCount = UnityEngine.Random.Range(TrainLengthVector.x, TrainLengthVector.y);

            var vanXPos = 0f;
            var lastVanWidth = 0f;
            for (var i = 0; i < vansCount; i++) {
                var vanPrefab = i > 0 ? VanPrefabs[UnityEngine.Random.Range(0, VanPrefabs.Length)] : MainVan;
                var van = Instantiate(vanPrefab, trainobj.transform);
                var vanCollider = van.GetComponentInChildren<Collider2D>();
                var vanWidth = vanCollider.bounds.size.x;
                var vanWidthAdd = 0f;
                var space = 0f;
                if (i > 0) {
                    vanWidthAdd = vanWidth;
                    space = UnityEngine.Random.Range(SpaceBetweenVansVector.x, SpaceBetweenVansVector.y);
                }
                vanXPos -= (lastVanWidth / 2 + vanWidthAdd / 2 + space) / trainobj.transform.localScale.x;
                lastVanWidth = vanWidth;
                van.transform.localPosition = new Vector3(vanXPos, 0, 0);
            }
        }
    }
}
