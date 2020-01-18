using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using UnityEngine;
using InputSystem;
using Stickman.Movement;

namespace Stickman.Controllers {
    public class PlayerController : MonoBehaviour {

        public int Id;
        //private MuscleController _MuscleController;
        private MovementController _MovementController;
        private InputKit _InputKit;

        private void Awake() {
            _MovementController = GetComponent<MovementController>();
        }

        private void Start() {
            _InputKit = InputConfig.Instance.GetSettings(Id);
        }

        public void Update() {
            var hor = Input.GetAxis(_InputKit.Horizontal);
            var vert = Input.GetAxis(_InputKit.Vertical);
            _MovementController.SetHorizontal(hor);
            //if (Input.GetKeyDown(_InputKit.Attack1)) {
            //    _MuscleController.AttackHand();
            //}
            //if (Input.GetKeyDown(_InputKit.Attack2)) {
            //    _MuscleController.AttackLeg();
            //}
            if (Input.GetKeyDown(_InputKit.Jump))
            {
                _MovementController.Jump();
            }
        }
    }
}
