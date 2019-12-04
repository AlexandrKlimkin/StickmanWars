using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Unity;
using UnityEngine;

namespace Stickman.MuscleSystem {
    [Serializable]
    public class Muscle {
        public string Name;
        public MuscleType MuscleType;
        public Rigidbody2D Rigidbody;
        public Collider2D Collider;
        public float RestRotation;
        public float Force;
        public Transform TargetRotation;
        public bool Disabled = false;
        public BoneCollider BoneCollider;

        private float _AddRotation;
        private float _AddForce;
        private float _CurrentForce;

        private Coroutine _DisableForTimeCoroutine;

        public void Initialize() {
            Collider = Rigidbody.GetComponent<Collider2D>();
            BoneCollider = Rigidbody.GetComponent<BoneCollider>();
            _CurrentForce = Force;
        }

        public void ActivateMuscle() {
            if (Disabled)
                return;
            var rotation = TargetRotation ? TargetRotation.rotation.eulerAngles.z : RestRotation;
            RotateSmooth(rotation + _AddRotation, _CurrentForce + _AddForce);

            _AddRotation = 0;
            _AddForce = 0;
        }

        public void AddMuscleRot(float rot) {
            _AddRotation = rot;
        }

        public void AddMuscleForce(float force) {
            _AddForce = force;
        }

        public void AddForce(Vector2 force) {
            Rigidbody.AddForce(force);
        }

        public void AddForce(Vector2 force, Vector2 pos) {
            Rigidbody.AddForceAtPosition(force, pos);
        }

        public void Move(Vector2 moveVector) {
            Rigidbody.position += moveVector;
        }

        public void DisableForTime(float time, AnimationCurve curve) {
            if (_DisableForTimeCoroutine != null)
                UnityEventProvider.Instance.StopCoroutine(_DisableForTimeCoroutine);
            _DisableForTimeCoroutine = UnityEventProvider.Instance.StartCoroutine(DisableForTimeRoutine(time, curve));
        }

        private IEnumerator DisableForTimeRoutine(float time, AnimationCurve curve) {
            var endTime = Time.time + time;
            while (Time.time < endTime) {
                var progress = Mathf.Clamp((endTime - Time.time) / time, 0, float.MaxValue);
                _CurrentForce = Force * curve.Evaluate(progress);
                yield return null;
            }
            _CurrentForce = Force;
        }

        private void RotateSmooth(float rotation, float force) {
            float angle = Mathf.DeltaAngle(Rigidbody.rotation, rotation);
            float ratio = angle / 180;
            ratio *= ratio;
            Rigidbody.MoveRotation(Mathf.LerpAngle(Rigidbody.rotation, rotation, force * ratio * Time.fixedDeltaTime));
            Rigidbody.AddTorque(angle * force * (1 - ratio) * .1f);
        }
    }

    public enum MuscleType { Head, Hip, Chest, LegUp, LegDown, ArmUp, ArmDown, Neck }
}