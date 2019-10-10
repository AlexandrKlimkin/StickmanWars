using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuscleSystem {

    [Serializable]
    public class WalkAction : MuscleAction {

        private Muscle _Hip;
        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;

        [Header("Settings")]
        public float FirstWalkingPhaseTime = 0.25f;
        public float SecondWalkingPhaseTime = 0.1f;
        public float HipMoveForce;
        public float LegUpMoveForce;
        public float LegDownMoveForce;
        public float hipRot = 30;
        public float LegUpAddForce;
        public float LegDownAddForce;
        public float MoveSpeed;
        [Header("FirstState")]
        public float FirstLegUpAngle1 = 90f;
        public float FirstLegDownAngle1 = 10f;
        public float SecondLegUpAngle1 = 90;
        public float SecondLegDownAngle1 = -10f;
        [Header("SecondState")]
        public float FirstLegUpAngle2 = 90f;
        public float FirstLegDownAngle2 = 10f;
        public float SecondLegUpAngle2 = 90;
        public float SecondLegDownAngle2 = -10f;

        [Header("Debug")]
        [SerializeField]
        private int _State = 0;
        [SerializeField]
        private float _CurrentCycleTime = 0;
        [SerializeField]
        private float _TimePassed = 0;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            _Hip = muscles.FirstOrDefault(_=>_.MuscleType == MuscleType.Hip);
            _LegUp = muscles.Where(_=>_.MuscleType == MuscleType.LegUp).ToList();
            _LegDown = muscles.Where(_=>_.MuscleType == MuscleType.LegDown).ToList();
        }

        public override void UpdateAction(params float[] parameters) {
            var horizontal = parameters[0];

            int fl = _State > 1 ? 0 : 1;
            int sl = _State > 1 ? 1 : 0;

            _Hip.AddMuscleRot(hipRot * - horizontal);
            //_Hip.AddForce(new Vector2(_Horizontal * HipMoveForce, 0));

            _Muscles.Where(_ => _.MuscleType != MuscleType.ArmUp && _.MuscleType != MuscleType.ArmDown).ToList().ForEach(_ => _.Move(new Vector2(horizontal * MoveSpeed * Time.fixedDeltaTime, 0)));
            //_Muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList()[fl].Move(new Vector2(horizontal * MoveSpeed * Time.fixedDeltaTime, 0));
            //_Muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList()[fl].Move(new Vector2(horizontal * MoveSpeed * Time.fixedDeltaTime, 0));

            switch (_State) {
                case 2:
                case 0:
                _CurrentCycleTime = FirstWalkingPhaseTime;
                _LegUp[fl].AddMuscleRot(FirstLegUpAngle1 * horizontal);
                _LegDown[fl].AddMuscleRot(FirstLegDownAngle1 * horizontal);
                _LegUp[sl].AddMuscleRot(SecondLegUpAngle1 * horizontal);
                _LegDown[sl].AddMuscleRot(SecondLegDownAngle1 * horizontal);

                break;

                case 3:
                case 1:
                _CurrentCycleTime = SecondWalkingPhaseTime;
                _LegUp[fl].AddMuscleRot(FirstLegUpAngle2 * horizontal);
                _LegDown[fl].AddMuscleRot(FirstLegDownAngle2 * horizontal);
                _LegUp[sl].AddMuscleRot(SecondLegUpAngle2 * horizontal);
                _LegDown[sl].AddMuscleRot(SecondLegDownAngle2 * horizontal);
                break;
            }
            _TimePassed += Time.fixedDeltaTime;
            if (_TimePassed > _CurrentCycleTime) {
                _TimePassed = 0;
                _State++;
                if (_State > 3)
                    _State = 0;
            }
        }
    }
}
