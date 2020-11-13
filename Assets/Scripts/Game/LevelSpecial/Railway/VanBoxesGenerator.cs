using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class VanBoxesGenerator : MonoBehaviour {

        public Rigidbody2D BoxPrefab;
        public float BoxWidth;
        public float BoxHeight;
        public Transform StartPoint1;
        public Transform StartPoint2;
        public Transform FinishPoint1;
        public Transform FinishPoint2;
        [Header("Parameters")]
        public Vector2 SmoothnessRandVector;
        public float MaxHeight;
        public float YAddiction;

        private Rigidbody2D _Van;

        public void GenerateBoxes(Rigidbody2D van) {
            _Van = van;
            var startX = StartPoint1.position.x;
            var startY = StartPoint1.position.y;

            var startPos = Random.Range(StartPoint1.position.x, StartPoint2.position.x);
            var finishPos = Random.Range(FinishPoint1.position.x, FinishPoint2.position.x);

            var width = (int)((finishPos - startPos) / BoxWidth);
            var smoothnes = Random.Range(SmoothnessRandVector.x, SmoothnessRandVector.y);
            var amplitude = MaxHeight - YAddiction;
            var xPos = startX;
            var xPerlin = Random.Range(0, 100f);
            var yPerlin = Random.Range(0, 100f);
            xPos += BoxWidth / 2f;
            for (var i = 0; i < width; i++) {
                var perlinNoise = Mathf.PerlinNoise(xPerlin, yPerlin);
                //Debug.LogError(perlinNoise);
                var height = (int)(perlinNoise * (amplitude + 1)) + YAddiction;
                var yPos = startY + BoxHeight / 2f;
                for (var j = 0; j < height; j++) {
                    var box = Instantiate(BoxPrefab, ObstaclesContainer.Instance.transform);
                    box.transform.position = new Vector3(xPos, yPos);
                    box.velocity = _Van.velocity;
                    yPos += BoxHeight;
                }
                xPerlin += BoxWidth * smoothnes;
                xPos += BoxWidth;
            }
        }
    }
}
