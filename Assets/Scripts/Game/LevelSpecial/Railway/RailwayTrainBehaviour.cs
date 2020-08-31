using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial {
    public class RailwayTrainBehaviour : MonoBehaviour {

        public PathFollowPhysicsObject Train;
        public Vector2 DelayTimeRandom;

        private void Start() {
            StartCoroutine(TrainRoutine());
        }

        private IEnumerator TrainRoutine() {
            while (true) {
                yield return new WaitUntil(() => !Train.IsMoving);
                yield return new WaitForSeconds(Random.Range(DelayTimeRandom.x, DelayTimeRandom.y));
                Train.ResetToPoint(0);
                Train.MoveToNextPoint();
            }
        }
    }
}
