using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace MuscleSystem {
    public class MuscleController : MonoBehaviour {
        [SerializeField]
        private List<Muscle> _Muscles;

        private List<Muscle> _LegUp;
        private List<Muscle> _LegDown;
        private Muscle _Hip;

        private float _Horizontal;

        private float _Direction;

        [SerializeField]
        private WalkAction _WalkAction;
        [SerializeField]
        private JumpAction _JumpAction;
        [SerializeField]
        private AttackAction _AttackAction;
        [SerializeField]
        private AttackLegAction _AttackLegAction;

        private Camera _Camera;

        private void Start() {
            //_LegUp = _Muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList();
            //_LegDown = _Muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList();
            //_Hip = _Muscles.First(_ => _.MuscleType == MuscleType.Hip);
            _Camera = Camera.main;
            RegisterActions();
        }

        private void RegisterActions() {
            _WalkAction.Initialize(_Muscles);
            _JumpAction.Initialize(_Muscles);
            _AttackAction.Initialize(_Muscles);
            _AttackLegAction.Initialize(_Muscles);
        }

        private void Update() {

            var mousePos = _Camera.ScreenToWorldPoint(Input.mousePosition);
            var dir = mousePos.x > transform.position.x ? 1 : -1;
            _Horizontal = Input.GetAxis("Horizontal");
            if (_Horizontal != 0)
                _Direction = _Horizontal > 0 ? 1 : -1;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1)) {
                _JumpAction.UpdateAction(1f);
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Joystick1Button0)) {
                _AttackAction.UpdateAction(dir);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Joystick1Button2)) {
                _AttackLegAction.UpdateAction(dir);
            }
        }

        private void FixedUpdate() {
            if (_Horizontal != 0) {
                _WalkAction.UpdateAction(_Horizontal);
            }
            _Muscles.ForEach(_ => _.ActivateMuscle());
        }
    }
}