using Stickman.MuscleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputSystem;

namespace Stickman.Controllers {
    public class PlayerController : MonoBehaviour {

        public int Id;
        private MuscleController _MuscleController;
        private InputKit _InputKit;

        private void Awake() {
            _MuscleController = GetComponent<MuscleController>();
        }

        private void Start() {
            _InputKit = InputConfig.Instance.GetSettings(Id);
        }

        public void Update() {
            var hor = Input.GetAxis(_InputKit.Horizontal);
            var vert = Input.GetAxis(_InputKit.Vertical);
            _MuscleController.SetHorizontal(hor);
            if (Input.GetKeyDown(_InputKit.Attack1)) {
                _MuscleController.AttackHand();
            }
            if (Input.GetKeyDown(_InputKit.Attack2)) {
                _MuscleController.AttackLeg();
            }
            if (Input.GetKeyDown(_InputKit.Jump)) {
                _MuscleController.Jump();
            }
        }
    }
}
