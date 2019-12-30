using Assets.Scripts.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Unity;
using UnityEngine;

namespace Character.MuscleSystem {
    [Serializable]
    public class Muscle {
        public string Name;
        public MuscleType MuscleType;
        public Rigidbody2D Rigidbody;
        public HingeJoint2D Joint;
        public Collider2D Collider;
        public float RestRotation;
        public float Force;
        public Transform TargetRotation;
        public bool Disabled = false;
        public BoneCollider BoneCollider;
        public Transform ViewTransform;

        private float _AddRotation;
        private float _AddForce;
        private float _CurrentForce;

        private Coroutine _DisableForTimeCoroutine;
        private Vector3 _DeltaAxis;
        //private JointMotor2D _StartMotor;

        public void Initialize() {
            Collider = Rigidbody.GetComponentInChildren<Collider2D>();
            BoneCollider = Rigidbody.GetComponentInChildren<BoneCollider>();
            Joint = Rigidbody.GetComponentInChildren<HingeJoint2D>();
            ViewTransform = Rigidbody.transform;
            _CurrentForce = Force;
            //if(Joint)
            //    _StartMotor = Joint.motor;
        }

        public void ActivateMuscle() {
            if (Disabled)
                return;
            var rotation = TargetRotation ? TargetRotation.rotation.eulerAngles.z : RestRotation;
            RotateSmooth(rotation + _AddRotation, _CurrentForce + _AddForce);
            //if(MuscleType == MuscleType.HipUp)
            //    RotateSmooth(rotation + _AddRotation, _CurrentForce + _AddForce);
            //else
            //    RotateJoints(rotation + _AddRotation, _CurrentForce + _AddForce);

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

        public void ChangeDirection() {

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
            //if (ConnectedAxis != null && Axis != null) {
            //    _DeltaAxis = ConnectedAxis.position - Axis.position;
            //    var newPos = Rigidbody.position + _DeltaAxis.ToVector2();
            //    Rigidbody.position = newPos;
            //    Rigidbody.transform.position = newPos;
            //}
        }

        //private void RotateJoints(float rotation, float force) {
        //    if (Joint == null)
        //        return;
        //    float dir = rotation > Rigidbody.rotation ? -1f : 1f;
        //    Joint.motor = new JointMotor2D() {
        //        maxMotorTorque = _StartMotor.maxMotorTorque,
        //        motorSpeed = _StartMotor.motorSpeed * dir
        //    };
        //    if (ConnectedAxis != null && Axis != null) {
        //        _DeltaAxis = ConnectedAxis.position - Axis.position;
        //        var newPos = Rigidbody.position + _DeltaAxis.ToVector2();
        //        Rigidbody.position = newPos;
        //        Rigidbody.transform.position = newPos;
        //    }
        //}
    }

    public enum MuscleType { Head, HipUp, Chest, LegUp, LegDown, ArmUp, ArmDown, Neck, Fist, Boot, HipDown, LegMiddle } // ToDo:Bone type/muscle type
}