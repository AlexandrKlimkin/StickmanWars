using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stickman.MuscleSystem {

    [Serializable]
    public class JumpAction : MuscleAction {
        [Header("Settings")]
        public float JumpForce;
        public float GroundedDist = 0.5f;

        [Interval(0f, 20f)]
        public Vector2 PoseHeightRanges;

        [Header("FirstState")]
        public float FirstLegUpAngle1 = 90f;
        public float FirstLegDownAngle1 = -10f;
        public float SecondLegUpAngle1 = 90;
        public float SecondLegDownAngle1 = -10f;
        public float HipAngle1 = -10f;
        public float ChestAngle1 = -10f;

        private Muscle _Hip;
        private Muscle _Chest;
        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            _Hip = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.Hip);
            _Chest = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.Chest);
            _LegUp = muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList();
            _LegDown = muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList();
        }

        public override void UpdateAction(params float[] parameters) {
            var distance = parameters[0];
            var dir = parameters[1];
            var cof = Mathf.InverseLerp(PoseHeightRanges.x, PoseHeightRanges.y, distance);

            int fl = 0;
            int sl = 1;
            _Hip.AddMuscleRot(HipAngle1 * dir * cof);
            _Chest.AddMuscleRot(ChestAngle1 * dir * cof);
            _LegUp[fl].AddMuscleRot(FirstLegUpAngle1 * dir * cof);
            _LegDown[fl].AddMuscleRot(FirstLegDownAngle1 * dir * cof);
            _LegUp[sl].AddMuscleRot(SecondLegUpAngle1 * dir * cof);
            _LegDown[sl].AddMuscleRot(SecondLegDownAngle1 * dir * cof);
        }

        public void Jump() {
            _Muscles.ForEach(muscle => muscle.AddForce(new Vector2(0, JumpForce)));
        }
    }
}
