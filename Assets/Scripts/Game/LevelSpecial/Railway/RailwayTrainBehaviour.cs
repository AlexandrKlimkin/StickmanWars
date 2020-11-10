using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.LevelSpecial {
    public class RailwayTrainBehaviour : MonoBehaviour {

        public PathFollowPhysicsObject Train;
        public Vector2 DelayTimeRandom;
        public List<TrafficLightController> TrafficLights;

        private void Start() {
            StartCoroutine(TrainRoutine());
        }

        private IEnumerator TrainRoutine() {
            while (true) {
                TrafficLights.ForEach(_ => _.SetState(TrafficLightController.TrafficLightControllerState.Off));
                var delay = Random.Range(DelayTimeRandom.x, DelayTimeRandom.y);
                //Debug.LogError($"Waiting for delay {delay} sec");
                yield return new WaitForSeconds(delay);
                var randpoint = Random.Range(0, 1);
                TrafficLights[randpoint].SetState(TrafficLightController.TrafficLightControllerState.On);
                Train.ResetToPoint(randpoint);
                //Debug.LogError($"Reset To Point {randpoint}");
                var scaleCof = randpoint == 0 ? 1 : -1;
                var scale = Train.transform.localScale;
                Train.transform.localScale = new Vector3(Mathf.Abs(scale.x) * scaleCof, scale.y, scale.z);
                yield return new WaitForFixedUpdate();
                Train.MoveToNextPoint();
                //Debug.LogError("Start Moving");
                yield return new WaitUntil(() => !Train.IsMoving);
                //Debug.LogError("Not Moving");
            }
        }
    }
}
