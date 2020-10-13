using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Control;
using Character.Movement.Modules;
using Character.Shooting;
using Tools.BehaviourTree;
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
        private PushingParameters PushingParameters;
        [SerializeField]
        private JumpParameters JumpParameters;

        private List<MovementModule> _MovementModules;
        private WalkModule _WalkModule;
        private GroundCheckModule _GroundCheckModule;
        private WallsCheckModule _WallsCheckModule;
        private WallsSlideModule _WallsSlideModule;
        private JumpModule _JumpModule;
        private LedgeHangModule _LedgeHangModule;
        private PushingModule _PushingModule;
        private Blackboard _Blackboard;


        private WalkData _WalkData;
        private List<SimpleCCD> _SimpleCcds = new List<SimpleCCD>();

        public CharacterUnit Owner { get; private set; }
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
        public bool WallRun => _WallsSlideModule.WallRun;
        public bool LedgeHang => _LedgeHangModule.LedgeHang;
        public bool Pushing => _PushingModule.Pushing;
        public float TimeFallingDown => _GroundCheckModule.TimeFallingDown;
        public float TimeNotFallingDown => _GroundCheckModule.TimeNotFallingDown;

        public event Action OnPressJump;
        public event Action OnHoldJump;
        public event Action OnReleaseJump;

        private void Awake() {
            Owner = GetComponent<CharacterUnit>();
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
            _PushingModule = new PushingModule(PushingParameters);

            _MovementModules.Add(_GroundCheckModule);
            _MovementModules.Add(_WallsCheckModule);
            _MovementModules.Add(_WalkModule);
            _MovementModules.Add(_LedgeHangModule);
            _MovementModules.Add(_WallsSlideModule);
            _MovementModules.Add(_JumpModule);
            _MovementModules.Add(_PushingModule);
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
            commonData.WeaponController = Owner.WeaponController;
        }

        private float _JumpTimer;
        private bool _JumpHold;
        private const float PressTime2HighJump = 0.12f;

        private void Update() {
            _MovementModules.ForEach(_ => _.Update());
        }

        private void LateUpdate() {
            _MovementModules.ForEach(_ => _.LateUpdate());
            _JumpHold = false;
        }

        private void FixedUpdate() {
            _MovementModules.ForEach(_ => _.FixedUpdate());
        }

        public void SetHorizontal(float hor) {
            _WalkModule.SetHorizontal(hor);
        }
        private bool _Jumping = false;

        public bool Jump() {
            if (Owner.WeaponController.HasVehicle && Owner.WeaponController.Vehicle.InputProcessor.CurrentMagazine != 0)
                return false;
            _Jumping = _JumpModule.Jump(this);
            if (_Jumping) {
                _JumpTimer = 0;
            }
            return _Jumping;
        }

        public bool HighJump() {
            var jumped = false;
             jumped = Jump();
            if (!jumped)
                jumped = WallJump();
            //Debug.LogError($"Jumped {jumped}");
            if (jumped) {
                StopCoroutine(ContinueJumpRoutine());
                StartCoroutine(ContinueJumpRoutine());
            }
            return jumped;
        }

        private IEnumerator ContinueJumpRoutine() {
            var continueJump = false;
            while (!continueJump) {
                continueJump = ProcessHoldJump();
                yield return null;
            }
        }

        private void ContinueJump() {
            if (Owner.WeaponController.HasVehicle && Owner.WeaponController.Vehicle.InputProcessor.CurrentMagazine != 0)
                return;
            //Debug.LogError("Continue jump");
            _JumpModule.ContinueJump();
            _Jumping = false;
        }

        public bool WallJump() {
            if (Owner.WeaponController.HasVehicle && Owner.WeaponController.Vehicle.InputProcessor.CurrentMagazine != 0)
                return false;
            return _JumpModule.WallJump(this);
        }

        public void ContinueWallJump() {
            _JumpModule.ContinueWallJump();
        }

        public void ChangeDirection(int newDir) {
            if (_WalkData.Direction == newDir) //_WalkData.Direction == newDir
                return;
            _WalkData.Direction = newDir;
            var localScale = WalkParameters.IkTransform.localScale;
            var newLocalScale = new Vector3(newDir * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            WalkParameters.IkTransform.localScale = newLocalScale;
            _SimpleCcds.ForEach(_ => _.ReflectNodes());
        }

        public void SubscribeWeaponOnEvents(Weapon weapon) {
            OnHoldJump += weapon.InputProcessor.ProcessHold;
            OnPressJump += weapon.InputProcessor.ProcessPress;
            OnReleaseJump += weapon.InputProcessor.ProcessRelease;
        }

        public void UnSubscribeWeaponOnEvents(Weapon weapon) {
            OnHoldJump -= weapon.InputProcessor.ProcessHold;
            OnPressJump -= weapon.InputProcessor.ProcessPress;
            OnReleaseJump -= weapon.InputProcessor.ProcessRelease;
        }

        public bool ProcessHoldJump() {
            _JumpHold = true;
            _JumpTimer += Time.deltaTime;
            if (_Jumping && _JumpTimer > PressTime2HighJump) {
                ContinueJump();
                _JumpTimer = 0;
                return true;
            }
            OnHoldJump?.Invoke();
            return false;
        }

        public void PressJump() {
            OnPressJump?.Invoke();
        }

        public void ReleaseJump() {
            OnReleaseJump?.Invoke();
        }

    }
}
