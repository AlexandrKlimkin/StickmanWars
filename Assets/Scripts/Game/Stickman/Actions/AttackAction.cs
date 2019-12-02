using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Stickman.MuscleSystem {
    [Serializable]
    public class AttackAction : MuscleAction {

        private List<Muscle> _ArmUp;
        private List<Muscle> _ArmDown;
        private Muscle _Chest;

        [Header("Settings")]
        public float HandForce;
        public float HitForce;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            _ArmDown = muscles.Where(_ => _.MuscleType == MuscleType.ArmDown).ToList();
            _ArmUp = muscles.Where(_=> _.MuscleType == MuscleType.ArmUp).ToList();
            _Chest = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.Chest);
        }

        [Header("Debug")]
        [SerializeField]
        private int _Arm = 0;

        public override void UpdateAction(params float[] parameters) {
            _ArmDown[_Arm].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
            _Arm = _Arm == 0 ? 1 : 0;
            var direction = parameters[0];
            var arm = _ArmDown[_Arm];
            var point = _Chest.Rigidbody.position + Vector2.right;
            var dir = point - _Chest.Rigidbody.position;
            var force = dir * direction * HandForce;
            _ArmDown[_Arm].BoneCollider.DamageableCollisionEnter += OnHitDamageable;
            arm.AddForce(force);

        }

        private void OnHitDamageable(Collision2D collision) {
            _ArmDown[_Arm].BoneCollider.DamageableCollisionEnter -= OnHitDamageable;
            var contact = collision.contacts[0]; //ToDo
            collision.rigidbody.AddForceAtPosition(-contact.normal * HitForce, contact.point);
        }
    }
}
