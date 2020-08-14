using Assets.Scripts.Tools;
using Character.Health;
using Character.Movement;
using Character.Shooting;
using KlimLib.SignalBus;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.AI {
    public class BotBehaviour : CharacterBehaviour {

        [Dependency]
        private readonly SignalBus _SignalBus;

        private w2dp_PathCalculator calculator = new w2dp_PathCalculator();
        private List<w2dp_Waypoint> currentPath;
        private MovementData _MovementData;

        protected override BehaviourTree BuildBehaviourTree() {
            var behaviourTree = new BehaviourTree();
                var root = behaviourTree.AddChild<SelectorTask>();
                    var mainTree = root.AddChild<ParallelTask>();
                        var movement = mainTree.AddChild<ParallelTask>();
                            var combatDestination = movement.AddChild(new SelectorTask());
                                //combatDestination.AddChild(new WeaponDestinationTask());
                                //combatDestination.AddChild(new RandomPointDestinationTask());
                                combatDestination.AddChild(new TransformDestinationTask());
                            movement.AddChild(new MoveToPointTask());
                        var shooting = mainTree.AddChild<ParallelTask>();
                            //shooting.AddChild(new ShootTask());
            return behaviourTree;
        }

        protected override Blackboard BuildBlackboard() {
            var bb = new Blackboard();
            return bb;
        }

        protected override void Awake() {
            base.Awake();
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeathSignal, this);
            _MovementData = BehaviourTree.Blackboard.Get<MovementData>();
            CharacterUnit.WeaponController.OnWeaponEquiped += OnWeaponEquip;
            CharacterUnit.WeaponController.OnVehicleEquiped += OnVehicleEquiped;
        }

        private void OnWeaponEquip(Weapon weapon) {
            if(_MovementData.DestinationType == DestinationType.Weapon) {
                _MovementData.DestinationType = DestinationType.None;
                _MovementData.TargetPos = null;
                _MovementData.CurrentPath = null;
            }
        }

        private void OnVehicleEquiped(Weapon weapon) {

        }

        private void OnCharacterDeathSignal(CharacterDeathSignal signal) {
            if (signal.Damage.Receiver != CharacterUnit as IDamageable)
                return;
            foreach (var task in BehaviourTree.Tasks) {
                if (task is BuildedTask)
                    ((BuildedTask)task).UpdatedTask = false;
            }
        }

        protected virtual void OnDestoy() {
            CharacterUnit.WeaponController.OnWeaponEquiped -= OnWeaponEquip;
            CharacterUnit.WeaponController.OnVehicleEquiped -= OnVehicleEquiped;
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void Update() {
            UpdateBT();
        }

        private void OnDrawGizmos() {
            if (BehaviourTree == null)
                return;
            var movementData = BehaviourTree.Blackboard.Get<MovementData>();
            if (movementData.TargetPos != null) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(movementData.TargetPos.Value, 10f);
            }

            Gizmos.color = Color.blue;
            if (_MovementData.CurrentPointPath != null) {
                for (int i = 0; i < _MovementData.CurrentPointPath.Count - 1; i++) {
                    var point1 = _MovementData.CurrentPointPath[i];
                    var point2 = _MovementData.CurrentPointPath[i + 1];
                    Gizmos.DrawLine(point1 + Vector3.up * 3, point2 + Vector3.up * 3);
                }
                if (_MovementData.CurrentPointPath.Count > 0) {
                    Gizmos.DrawLine(CharacterUnit.transform.position + Vector3.up * 3, _MovementData.CurrentPointPath[0] + Vector3.up * 3);
                }
            }
        }
    }
}
