using MuscleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MuscleSystem {

    [Serializable]
    public class JumpAction : MuscleAction {
        [Header("Settings")]
        public float JumpForce;
        [Interval(0f, 20f)]
        public Vector2 PoseHeightRanges;

        [Header("FirstState")]
        public float FirstLegUpAngle1 = 90f;
        public float FirstLegDownAngle1 = 10f;
        public float SecondLegUpAngle1 = 90;
        public float SecondLegDownAngle1 = -10f;

        private Muscle _Hip;
        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            _Hip = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.Hip);
            _LegUp = muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList();
            _LegDown = muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList();
        }

        public override void UpdateAction(params float[] parameters) {
            var jumpForce = parameters[0];
            _Muscles.ForEach(muscle => muscle.AddForce(new Vector2(0, jumpForce * JumpForce)));

            var horizontal = 1f;

            int fl = 0;
            int sl = 1;

            _LegUp[fl].AddMuscleRot(FirstLegUpAngle1 * horizontal);
            _LegDown[fl].AddMuscleRot(FirstLegDownAngle1 * horizontal);
            _LegUp[sl].AddMuscleRot(SecondLegUpAngle1 * horizontal);
            _LegDown[sl].AddMuscleRot(SecondLegDownAngle1 * horizontal);

        }
    }
}
