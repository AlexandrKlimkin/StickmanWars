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
        public float Force;

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
            var direction = parameters[0];
            var arm = _ArmDown[_Arm];
            var point = _Chest.Rigidbody.position + Vector2.right;
            var dir = point - _Chest.Rigidbody.position;
            var force = dir * direction * Force;
            arm.AddForce(force);
            _Arm = _Arm == 0 ? 1 : 0;
        }

    }
}
