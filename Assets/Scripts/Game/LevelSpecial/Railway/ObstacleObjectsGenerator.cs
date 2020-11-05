using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class ObstacleObjectsGenerator : MonoBehaviour {

        public List<Transform> Points;
        public List<GameObject> Obstacles;
        public Vector2Int ObstaclesAmountVector;

        private void Start() {
            GenerateObstacles();
        }

        private void GenerateObstacles() {
            if(Points == null || Points.Count == 0)
                return;
            if(Obstacles == null || Obstacles.Count == 0)
                return;
            var obstaclesCount = Random.Range(ObstaclesAmountVector.x, ObstaclesAmountVector.y + 1);
            var availablePointsToSpawn = Points.ToList();

            for (var i = 0; i < obstaclesCount; i++) {
                if(availablePointsToSpawn.Count == 0)
                    break;
                var randObstacleIndex = Random.Range(0, Obstacles.Count);
                var randObstacle = Obstacles[randObstacleIndex];
                var randPointIndex = Random.Range(0, availablePointsToSpawn.Count);
                var randomPoint = availablePointsToSpawn[randPointIndex];
                availablePointsToSpawn.RemoveAt(randPointIndex);
                Instantiate(randObstacle, randomPoint.position, randomPoint.rotation);
            }
        }
    }
}
