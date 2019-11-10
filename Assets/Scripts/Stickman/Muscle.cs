using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleSystem {
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

        public void Initialize() {
            Collider = Rigidbody.GetComponent<Collider2D>();
            BoneCollider = Rigidbody.GetComponent<BoneCollider>();
        }

        public void ActivateMuscle() {
            if (Disabled)
                return;
            var rotation = TargetRotation ? TargetRotation.rotation.eulerAngles.z : RestRotation;
            RotateSmooth(rotation + _AddRotation, Force + _AddForce);

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