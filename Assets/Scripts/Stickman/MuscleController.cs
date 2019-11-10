using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace MuscleSystem {
    public class MuscleController : MonoBehaviour {
        [SerializeField]
        private List<Muscle> _Muscles;

        [SerializeField]
        private WalkAction _WalkAction;
        [SerializeField]
        private JumpAction _JumpAction;
        [SerializeField]
        private AttackAction _AttackAction;
        [SerializeField]
        private AttackLegAction _AttackLegAction;
        [SerializeField]
        private InertionStopAction _InertionStopAction;

        [Header("GroundPoints")]
        [SerializeField]
        private List<Transform> LegPoints = new List<Transform>();
        [SerializeField]
        private float _GroundedDist;

        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;
        private Muscle _Hip;

        private float _Horizontal;
        private float _Direction;
        private bool _IsGrounded;
        private float _DistanceToGround;

        private Camera _Camera;

        private void Start() {
            //_LegUp = _Muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList();
            //_LegDown = _Muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList();
            _Hip = _Muscles.First(_ => _.MuscleType == MuscleType.Hip);
            _Camera = Camera.main;
            RegisterMuscles();
            RegisterActions();
        }

        private void RegisterMuscles() {
            _Muscles.ForEach(_ => _.Initialize());
        }

        private void RegisterActions() {
            _WalkAction.Initialize(_Muscles);
            _JumpAction.Initialize(_Muscles);
            _AttackAction.Initialize(_Muscles);
            _AttackLegAction.Initialize(_Muscles);
            _InertionStopAction.Initialize(_Muscles);
        }

        private void Update() {
            _IsGrounded = false;
            var mousePos = _Camera.ScreenToWorldPoint(Input.mousePosition);
            var dir = mousePos.x > transform.position.x ? 1 : -1;
            _Horizontal = Input.GetAxis("Horizontal");
            if (_Horizontal != 0)
                _Direction = _Horizontal > 0 ? 1 : -1;
            //if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Joystick1Button2)) {
            //    _AttackLegAction.UpdateAction(dir);
            //}
            _DistanceToGround = float.PositiveInfinity;
            foreach (var legPoint in LegPoints) {
                var hit = Physics2D.Linecast(legPoint.position, legPoint.position - new Vector3(0,1f,0) * 1000f, Layers.Masks.Walkable);
                if (hit.collider != null) {
                    Debug.DrawLine(hit.point - new Vector2(0.5f, 0), hit.point + new Vector2(0.5f, 0), Color.red);
                    Debug.DrawLine(hit.point - new Vector2(0, 0.5f), hit.point + new Vector2(0, 0.5f), Color.red);
                    if (_DistanceToGround > hit.distance)
                        _DistanceToGround = hit.distance;
                }
            }
            if (_DistanceToGround < _GroundedDist)
                _IsGrounded = true;
            if (_IsGrounded && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1)) {
                _JumpAction.Jump();
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                if (_IsGrounded)
                    _AttackAction.UpdateAction(1f);
                else
                    _AttackLegAction.UpdateAction(1f);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (_IsGrounded)
                    _AttackAction.UpdateAction(-1f);
                else
                    _AttackLegAction.UpdateAction(-1f);
            }
            Debug.Log(_DistanceToGround);
        }

        private void FixedUpdate() {
            if (!_IsGrounded) {
                _JumpAction.UpdateAction(_DistanceToGround, _Direction);
            }
            else {
                if (_Horizontal != 0) {
                    _WalkAction.UpdateAction(_Horizontal);
                }
                else {
                    _WalkAction.ResetState();
                }
            }
            _WalkAction.Push(_Horizontal);
            _Muscles.ForEach(_ => _.ActivateMuscle());
            _InertionStopAction.UpdateAction();
        }
    }
}