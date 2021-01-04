using KlimLib.SignalBus;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class UkazatelController : MonoBehaviour {
        [Dependency]
        private readonly SignalBus _SignalBus;

        public Transform PointerJoint;
        public float DefaultRotation;
        public float IsComingRotation;
        public float RotationLerpSpeed;

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<TrainMovementSignal>(OnTrainIsComingSignal, this);
        }

        private void InDestroy() {
            _SignalBus?.UnSubscribeFromAll(this);
        }

        private void SetTrainIsComing(bool isComing) {
            StopAllCoroutines();
            StartCoroutine(IsComingRoutine(isComing));
        }

        private IEnumerator IsComingRoutine(bool isComing) {
            var targetRoration = isComing ? IsComingRotation : DefaultRotation;
            var eulerRot = PointerJoint.rotation.eulerAngles;
            while (Mathf.Abs(eulerRot.z - targetRoration) > 0.01) {
                eulerRot = PointerJoint.rotation.eulerAngles;
                var rotation = Mathf.Lerp(eulerRot.z, targetRoration, Time.deltaTime * RotationLerpSpeed);
                PointerJoint.rotation = Quaternion.Euler(eulerRot.x, eulerRot.y, rotation);
                yield return null;
            }
            PointerJoint.rotation = Quaternion.Euler(eulerRot.x, eulerRot.y, targetRoration);
        }

        private void OnTrainIsComingSignal(TrainMovementSignal signal) {
            SetTrainIsComing(signal.Moving);
        }
    }
}