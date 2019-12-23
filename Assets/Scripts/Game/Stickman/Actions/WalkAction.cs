using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.MuscleSystem {
    [Serializable]
    public class WalkAction : MuscleAction {

        [Header("Settings")]
        public float FirstWalkingPhaseTime = 0.25f;
        public float SecondWalkingPhaseTime = 0.1f;
        public float ThirdWalkingPhaseTime = 0.1f;

        public float HipMoveForce;
        public float MaxMoveSpeed;
        public float DownForce;

        public float hipRot = 30;
        public float LegUpAddMuscleForce;
        public float LegDownAddMuscleForce;
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
        [Header("ThirdState")]
        public float FirstLegUpAngle3 = 90f;
        public float FirstLegDownAngle3 = 10f;
        public float SecondLegUpAngle3 = 90;
        public float SecondLegDownAngle3 = -10f;

        [Header("BrakeState")]
        [Interval(0f, 20f)]
        public Vector2 InterpotationRanges;
        public float HipAngle4 = 0f;
        public float ChestAngle4 = 0f;
        public float FirstLegUpAngle4= 40f;
        public float FirstLegDownAngle4 = 40f;
        public float SecondLegUpAngle4 = 40;
        public float SecondLegDownAngle4 = 40f;

        [Header("Debug")]
        [SerializeField]
        private int _State = 0;
        [SerializeField]
        private float _CurrentCycleTime = 0;
        [SerializeField]
        private float _TimePassed = 0;

        private Muscle _Hip;
        private Muscle _Chest;
        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;
        private float _PreviousHorizontal;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            _Hip = muscles.FirstOrDefault(_=>_.MuscleType == MuscleType.HipUp);
            _Chest = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.Chest);
            _LegUp = muscles.Where(_=>_.MuscleType == MuscleType.LegUp).ToList();
            _LegDown = muscles.Where(_=>_.MuscleType == MuscleType.LegDown).ToList();
        }

        public void ResetState() {
            _State = 0;
            _TimePassed = 0;
        }

        public void Push(float horizontal) {
            _Hip.AddMuscleRot(hipRot * -horizontal);
            _Hip.AddForce(new Vector2(0, -DownForce));
            if (horizontal > 0) {
                if (_Hip.Rigidbody.velocity.x < MaxMoveSpeed) {
                    _Hip.AddForce(new Vector2(horizontal * HipMoveForce * Time.fixedDeltaTime, 0));
                }
            }
            else if (horizontal < 0) {
                if (_Hip.Rigidbody.velocity.x > -MaxMoveSpeed) {
                    _Hip.AddForce(new Vector2(horizontal * HipMoveForce * Time.fixedDeltaTime, 0));
                }
            }
        }

        public override void UpdateAction(params float[] parameters) {
            var horizontal = parameters[0];
            _Muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList().ForEach(_ => _.AddMuscleForce(LegUpAddMuscleForce));
            _Muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList().ForEach(_ => _.AddMuscleForce(LegDownAddMuscleForce));

            int fl = _State > 1 ? 0 : 1;
            int sl = _State > 1 ? 1 : 0;

            if (horizontal * _Hip.Rigidbody.velocity.x <= 0 || Mathf.Abs(_PreviousHorizontal) > Mathf.Abs(horizontal)) {
                var xVelocity = Mathf.Abs(_Hip.Rigidbody.velocity.x);
                var cof = Mathf.InverseLerp(InterpotationRanges.x, InterpotationRanges.y, xVelocity);
                cof *= Mathf.Sign(horizontal);

                _Hip.AddMuscleRot(HipAngle4 * cof);
                _Chest.AddMuscleRot(ChestAngle4 * cof);
                _LegUp[fl].AddMuscleRot(FirstLegUpAngle4 * cof);
                _LegDown[fl].AddMuscleRot(FirstLegDownAngle4 * cof);
                _LegUp[sl].AddMuscleRot(SecondLegUpAngle4 * cof);
                _LegDown[sl].AddMuscleRot(SecondLegDownAngle4 * cof);
            }
            else {
                _LegDown.ForEach(_ => _.BoneCollider.GroundCollisionStay -= SwitchState);
                switch (_State) {
                    case 0:
                    case 2:
                    _CurrentCycleTime = FirstWalkingPhaseTime;
                    _LegUp[fl].AddMuscleRot(FirstLegUpAngle1 * horizontal);
                    _LegDown[fl].AddMuscleRot(FirstLegDownAngle1 * horizontal);
                    _LegUp[sl].AddMuscleRot(SecondLegUpAngle1 * horizontal);
                    _LegDown[sl].AddMuscleRot(SecondLegDownAngle1 * horizontal);

                    //_LegDown[sl].BoneCollider.GroundCollisionStay += SwitchState;
                    SwitchState();
                    break;

                    case 1:
                    case 3:
                    _CurrentCycleTime = SecondWalkingPhaseTime;
                    _LegUp[fl].AddMuscleRot(FirstLegUpAngle2 * horizontal);
                    _LegDown[fl].AddMuscleRot(FirstLegDownAngle2 * horizontal);
                    _LegUp[sl].AddMuscleRot(SecondLegUpAngle2 * horizontal);
                    _LegDown[sl].AddMuscleRot(SecondLegDownAngle2 * horizontal);
                    _LegDown[fl].BoneCollider.GroundCollisionStay += SwitchState;
                    break;

                    //case 2:
                    //case 5:
                    //_CurrentCycleTime = ThirdWalkingPhaseTime;
                    //_LegUp[fl].AddMuscleRot(FirstLegUpAngle3 * horizontal);
                    //_LegDown[fl].AddMuscleRot(FirstLegDownAngle3 * horizontal);
                    //_LegUp[sl].AddMuscleRot(SecondLegUpAngle3 * horizontal);
                    //_LegDown[sl].AddMuscleRot(SecondLegDownAngle3 * horizontal);

                    ////_LegDown[fl].BoneCollider.GroundCollisionStay += SwitchState;
                    //SwitchState();
                    //break;
                }
                _TimePassed += Time.fixedDeltaTime;
                SwitchState();
            }
            _PreviousHorizontal = horizontal;
        }

        private void SwitchState() {
            if (_TimePassed > _CurrentCycleTime) {
                _TimePassed = 0;
                _State++;
                if (_State > 3)
                    _State = 0;
            }
        }
    }
}
