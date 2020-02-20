using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Movement.Modules;
using UnityEngine;

namespace Character.Movement {
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        private WalkParameters WalkParameters;
        [SerializeField]
        private GroundCheckParameters GroundCheckParameters;
        [SerializeField]
        private WallCheckParameters WallCheckParameters;
        [SerializeField]
        private WallsSlideParameters WallSlideParameters;
        [SerializeField]
        private JumpParameters JumpParameters;

        private List<MovementModule> _MovementModules;
        private WalkModule _WalkModule;
        private GroundCheckModule _GroundCheckModule;
        private WallsCheckModule _WallsCheckModule;
        private WallsSlideModule _WallsSlideModule;
        private JumpModule _JumpModule;
        private Blackboard _Blackboard;

        public Rigidbody2D Rigidbody { get; private set; }
        public Vector2 Velocity => Rigidbody.velocity;
        public float Horizontal => _WalkModule.Horizontal;
        public bool IsGrounded => _GroundCheckModule.IsGrounded;
        public float MinDistanceToGround => _GroundCheckModule.MinDistanceToGround;
        public bool FallingDown => _GroundCheckModule.FallingDown;
        public bool WallSliding => _WallsSlideModule.WallSliding;
        public float Direction => _WalkModule.Direction;

        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            InitializeModules();
            SetupBlackboard();
            _MovementModules.ForEach(_ => _.Start());
        }

        private void InitializeModules()
        {
            _MovementModules = new List<MovementModule>();
            _WalkModule = new WalkModule(WalkParameters);
            _GroundCheckModule = new GroundCheckModule(GroundCheckParameters);
            _WallsCheckModule = new WallsCheckModule(WallCheckParameters);
            _WallsSlideModule = new WallsSlideModule(WallSlideParameters);
            _JumpModule = new JumpModule(JumpParameters);

            _MovementModules.Add(_GroundCheckModule);
            _MovementModules.Add(_WallsCheckModule);
            _MovementModules.Add(_WalkModule);
            _MovementModules.Add(_WallsSlideModule);
        }

        private void SetupBlackboard()
        {
            _Blackboard = new Blackboard();
            _MovementModules.ForEach(_=>_.Initialize(_Blackboard));
        }

        private void Update() {
            _MovementModules.ForEach(_ => _.Update());
        }

        private void FixedUpdate()
        {
            _MovementModules.ForEach(_ => _.FixedUpdate());
        }

        public void SetHorizontal(float hor) {
            _WalkModule.SetHorizontal(hor);
        }

        public bool Jump()
        {
            return _JumpModule.Jump(this);
        }

        public void ContinueJump() {
            _JumpModule.ContinueJump();
        }

        public bool WallJump()
        {
            return _JumpModule.WallJump();
        }

        public void ContinueWallJump() {
            _JumpModule.ContinueWallJump();
        }
    }
}
