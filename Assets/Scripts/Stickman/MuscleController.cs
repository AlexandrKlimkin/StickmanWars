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

        [SerializeField]
        private WalkAction _WalkAction;
        [SerializeField]
        private JumpAction _JumpAction;

        private void Start() {
            //_LegUp = _Muscles.Where(_ => _.MuscleType == MuscleType.LegUp).ToList();
            //_LegDown = _Muscles.Where(_ => _.MuscleType == MuscleType.LegDown).ToList();
            //_Hip = _Muscles.First(_ => _.MuscleType == MuscleType.Hip);

            RegisterActions();
        }

        private void RegisterActions() {
            _WalkAction.Initialize(_Muscles);
            _JumpAction.Initialize(_Muscles);
        }

        private void Update() {
            _Horizontal = Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1)) {
                _JumpAction.UpdateAction(1f);
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