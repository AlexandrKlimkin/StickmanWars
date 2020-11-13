using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.LevelSpecial.Railway {
    public class VanObjectsGenerator : MonoBehaviour {

        public List<GeneratedObjectParameters> ObjectsParameters;

        private Rigidbody2D _Van;

        public void Generate(Rigidbody2D van) {
            _Van = van;
            ObjectsParameters?.ForEach(GenerateObject);
        }

        private void GenerateObject(GeneratedObjectParameters parameters) {
            var randCount = Random.Range(parameters.RandomCount.x, parameters.RandomCount.y);
            for (var i = 0; i < randCount; i++) {
                if (parameters.RandObjectList == null)
                    return;
                var randObjectsCount = parameters.RandObjectList.Count;
                if (randObjectsCount == 0)
                    return;
                var randObjIndex = UnityEngine.Random.Range(0, randObjectsCount);
                var objPrefab = parameters.RandObjectList[randObjIndex];
                var pointX = Random.Range(parameters.GenerationStart.position.x,
                    parameters.GenerationFinish.position.x);
                var pointY = Random.Range(parameters.GenerationStart.position.y,
                    parameters.GenerationFinish.position.y);
                var point = new Vector2(pointX, pointY);
                var hit = Physics2D.Linecast(point + Vector2.up * parameters.RaycastLocalPosY, point,
                    Layers.Masks.Walkable);
                if (hit.collider == null)
                    return;
                var obj = Instantiate(objPrefab, ObstaclesContainer.Instance.transform);
                var offset = Vector3.up * Random.Range(parameters.HeightRandVector.x, parameters.HeightRandVector.y);
                obj.transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z) + offset;
                obj.GetComponent<Rigidbody2D>().velocity = _Van.velocity;
            }
        }
    }

    [Serializable]
    public class GeneratedObjectParameters {
        public List<GameObject> RandObjectList;
        public Vector2 RandomCount;
        public float RaycastLocalPosY;
        public Vector2 HeightRandVector;
        public Transform GenerationStart;
        public Transform GenerationFinish;
    }
}