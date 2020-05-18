using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Movement.Modules;
using UnityEditor;
using UnityEngine;

namespace Character.Movement {
    public class MovementController : MonoBehaviour {
        [SerializeField]
        private WalkParameters WalkParameters;
        [SerializeField]
        private GroundCheckParameters GroundCheckParameters;
        [SerializeField]
        private WallCheckParameters WallCheckParameters;
        [SerializeField]
        private WallsSlideParameters WallSlideParameters;
        [SerializeField]
        private LedgeHangParameters LedgeHangParameters;
        [SerializeField]
        private JumpParameters JumpParameters;

        private List<MovementModule> _MovementModules;
        private WalkModule _WalkModule;
        private GroundCheckModule _GroundCheckModule;
        private WallsCheckModule _WallsCheckModule;
        private WallsSlideModule _WallsSlideModule;
        private JumpModule _JumpModule;
        private LedgeHangModule _LedgeHangModule;
        private Blackboard _Blackboard;

        private WalkData _WalkData;
        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();

        public Rigidbody2D Rigidbody { get; private set; }
        public Collider2D Collider { get; private set; }

        public Vector2 Velocity => Rigidbody.velocity;
        public float Horizontal => _WalkModule.Horizontal;
        public bool IsGrounded => _GroundCheckModule.IsGrounded;
        public bool IsMainGrounded => _GroundCheckModule.IsMainGrounded;
        public float MinDistanceToGround => _GroundCheckModule.MinDistanceToGround;
        public bool FallingDown => _GroundCheckModule.FallingDown;
        public bool WallSliding => _WallsSlideModule.WallSliding;
        public float Direction => _WalkModule.Direction;
        public bool WallRun => _JumpModule.WallRun;
        public bool LedgeHang => _LedgeHangModule.LedgeHang;

        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
        }

        private void Start() {
            InitializeModules();
            SetupBlackboard();
            SetupCommon();
            _WalkData = _Blackboard.Get<WalkData>();
            Collider.gameObject.GetComponentsInChildren(_SimpleCcds);
            _MovementModules.ForEach(_ => _.Start());
        }

        private void InitializeModules() {
            _MovementModules = new List<MovementModule>();

            _WalkModule = new WalkModule(WalkParameters);
            _GroundCheckModule = new GroundCheckModule(GroundCheckParameters);
            _WallsCheckModule = new WallsCheckModule(WallCheckParameters);
            _WallsSlideModule = new WallsSlideModule(WallSlideParameters);
            _JumpModule = new JumpModule(JumpParameters);
            _LedgeHangModule = new LedgeHangModule(LedgeHangParameters);

            _MovementModules.Add(_GroundCheckModule);
            _MovementModules.Add(_WallsCheckModule);
            _MovementModules.Add(_WalkModule);
            _MovementModules.Add(_LedgeHangModule);
            _MovementModules.Add(_WallsSlideModule);
            _MovementModules.Add(_JumpModule);
        }

        private void SetupBlackboard() {
            _Blackboard = new Blackboard();
            _MovementModules.ForEach(_ => _.Initialize(_Blackboard));
        }

        private void SetupCommon() {
            var commonData = _Blackboard.Get<CommonData>();
            commonData.ObjRigidbody = Rigidbody;
            commonData.ObjTransform = this.transform;
            commonData.Collider = Collider;
            commonData.MovementController = this;
        }

        private void Update() {
            _MovementModules.ForEach(_ => _.Update());
        }

        private void LateUpdate() {
            _MovementModules.ForEach(_ => _.LateUpdate());
        }

        private void FixedUpdate() {
            _MovementModules.ForEach(_ => _.FixedUpdate());
        }

        public void SetHorizontal(float hor) {
            _WalkModule.SetHorizontal(hor);
        }

        public bool Jump() {
            return _JumpModule.Jump(this);
        }

        public void ContinueJump() {
            _JumpModule.ContinueJump();
        }

        public bool WallJump() {
            return _JumpModule.WallJump();
        }

        public void ContinueWallJump() {
            _JumpModule.ContinueWallJump();
        }

        private void OnDrawGizmos() { }

        //private void SetDirection() {
        //    if (_WalkData.Horizontal != 0) {
        //        var newDir = _WalkData.Horizontal > 0 ? 1 : -1;
        //        if (_WallSlideData.LeftTouch)
        //            newDir = -1;
        //        else if (_WallSlideData.RightTouch)
        //            newDir = 1;
        //        if (_WalkData.Direction != newDir)
        //            ChangeDirection(newDir);
        //    }
        //}

        public void ChangeDirection(int newDir) {
            if (_WalkData.Direction == newDir)
                return;
            _WalkData.Direction = newDir;
            var localScale = WalkParameters.IkTransform.localScale;
            var newLocalScale = new Vector3(newDir * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            WalkParameters.IkTransform.localScale = newLocalScale;
            //PuppetTransform.localScale = newLocalScale;
            _SimpleCcds.ForEach(_ => _.ReflectNodes());
        }

    }
}
