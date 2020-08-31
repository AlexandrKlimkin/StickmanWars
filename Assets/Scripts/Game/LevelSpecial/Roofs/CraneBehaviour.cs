using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial.Roofs {
    public class CraneBehaviour : MonoBehaviour {
        public Vector2 DelayTimeRandom;
        public Vector2 DelayAfterRageRandom;
        public float DelayAfterAnimationStart;

        public PathFollowPhysicsObject Platform;
        public SimpleRotationController BigGear;
        public SimpleRotationController SmallGear;
        public Animator CraneMan;

        private bool _LastIsMoving;

        private void Start() {
            StartCoroutine(CraneRoutine());
        }

        private void Update() {

        }

        private IEnumerator CraneRoutine() {
            while (true) {
                yield return SwitchCraneRoutine();
                yield return new WaitForSeconds(Random.Range(DelayTimeRandom.x, DelayTimeRandom.y));
            }
        }

        private IEnumerator SwitchCraneRoutine() {
            CraneMan.SetTrigger("Rage");
            yield return new WaitForSeconds(Random.Range(DelayAfterRageRandom.x, DelayAfterRageRandom.y));
            CraneMan.SetTrigger("Change");
            yield return new WaitForSeconds(DelayAfterAnimationStart);
            Platform.MoveToNextPoint();
            var clockwise = Platform.CurrentFollowPointIndex == 0;
            BigGear.Rotate(!clockwise);
            SmallGear.Rotate(clockwise);
            yield return new WaitUntil(() => !Platform.IsMoving);
            BigGear.Stop();
            SmallGear.Stop();
        }
    }
}