using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MuscleController : MonoBehaviour
{
    [SerializeField]
    private List<Muscle> _Muscles;

    public List<Muscle> _LegUp;
    public List<Muscle> _LegDown;
    private Muscle _Hip;

    private float _Horizontal;

    private void Start() {
        _LegUp = _Muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList();
        _LegDown = _Muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList();
        _Hip = _Muscles.First(_ => _.MuscleType == MuscleType.Hip);
    }

    private void Update() {
        _Horizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if(_Horizontal != 0) {
            Walk();
        }
        _Muscles.ForEach(_ => _.ActivateMuscle());
    }

    [Header("Walking")]
    public float FirstWalkingPhaseTime = 0.25f;
    public float SecondWalkingPhaseTime = 0.1f;
    public float HipMoveForce;
    public float LegUpMoveForce;
    public float LegDownMoveForce;
    public float hipRot = 30;

    public float LegUpAddForce;
    public float LegDownAddForce;

    public float MoveSpeed;

    [Header("Debug")]
    public int _State = 0;
    private float _CurrentCycleTime = 0;
    float _TimePassed = 0;

    private void Walk() {
        int fl = _State > 1 ? 0 : 1;
        int sl = _State > 1 ? 1 : 0;

        _Hip.AddMuscleRot(hipRot * -_Horizontal);
        //_Hip.AddForce(new Vector2(_Horizontal * HipMoveForce, 0));

        _Muscles.Where(_=>_.MuscleType != MuscleType.ArmUp && _.MuscleType != MuscleType.ArmDown).ToList().ForEach(_=>_.Move(new Vector2(_Horizontal * MoveSpeed * Time.fixedDeltaTime, 0)));
        _Muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList()[fl].Move(new Vector2(_Horizontal * MoveSpeed * Time.fixedDeltaTime, 0));
        _Muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList()[fl].Move(new Vector2(_Horizontal * MoveSpeed * Time.fixedDeltaTime, 0));

        switch (_State) {
            case 2:
            case 0:
            _CurrentCycleTime = FirstWalkingPhaseTime;
            _LegUp[fl].AddMuscleRot(90 * _Horizontal);
            //_LegUp[fl].AddForce(new Vector2(_Horizontal * LegUpMoveForce, 0));
            //_LegUp[fl].AddMuscleForce(LegUpAddForce);
            //_LegUp[sl].AddMuscleRot(0);
            //_LegDown[fl].AddMuscleRot(0);
            _LegDown[fl].AddMuscleRot(-10 * _Horizontal);
            //_LegDown[fl].AddForce(new Vector2(_Horizontal * LegDownMoveForce, 0));
            //_LegDown[fl].AddMuscleForce(LegDownAddForce);
            break;

            case 3:
            case 1:
            _CurrentCycleTime = SecondWalkingPhaseTime;
            //_LegUp[fl].AddMuscleRot(0);
            _LegUp[sl].AddMuscleRot(90 * _Horizontal);
            //_LegUp[sl].AddForce(new Vector2(_Horizontal * LegUpMoveForce, 0));
            //_LegUp[sl].AddMuscleForce(LegUpAddForce);
            //_LegDown[fl].AddMuscleRot(0);
            _LegDown[sl].AddMuscleRot(-10 * _Horizontal);
            //_LegDown[sl].AddForce(new Vector2(_Horizontal * LegDownMoveForce, 0));
            //_LegDown[sl].AddMuscleForce(LegDownAddForce);
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

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        //_Muscles.ForEach();
    }

}
