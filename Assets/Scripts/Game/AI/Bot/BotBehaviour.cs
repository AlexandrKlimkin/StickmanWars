using Assets.Scripts.Tools;
using Character.Movement;
using Character.Shooting;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.AI {
    public class BotBehaviour : CharacterBehaviour {

        private w2dp_PathCalculator calculator = new w2dp_PathCalculator();
        private List<w2dp_Waypoint> currentPath;
        private MovementData _MovementData;

        protected override BehaviourTree BuildBehaviourTree() {
            var behaviourTree = new BehaviourTree();
                var root = behaviourTree.AddChild<SelectorTask>();
                    var mainTree = root.AddChild<ParallelTask>();
                        var movement = mainTree.AddChild<SelectorTask>();
                            var combatDestination = movement.AddChild(new SelectorTask());          
                                combatDestination.AddChild(new WeaponDestinationTask());
                                combatDestination.AddChild(new RandomPointDestinationTask());
                            movement.AddChild(new MoveToPointTask());
            return behaviourTree;
        }

        protected override Blackboard BuildBlackboard() {
            var bb = new Blackboard();
            return bb;
        }

        protected override void Awake() {
            base.Awake();
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

        protected override void OnDestoy() {
            base.OnDestoy();
            CharacterUnit.WeaponController.OnWeaponEquiped -= OnWeaponEquip;
            CharacterUnit.WeaponController.OnVehicleEquiped -= OnVehicleEquiped;
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
                Gizmos.DrawWireSphere(movementData.TargetPos.Value + Vector3.up * 20f, 10f);
            }
        }
    }
}
