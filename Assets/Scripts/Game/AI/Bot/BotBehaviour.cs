using Assets.Scripts.Tools;
using Character.Movement;
using Character.Shooting;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.AI {
    public class BotBehaviour : CharacterBehaviour {

        public Transform Target;

        private w2dp_PathCalculator calculator = new w2dp_PathCalculator();
        private List<w2dp_Waypoint> currentPath;

        protected override BehaviourTree BuildBehaviourTree() {
            var behaviourTree = new BehaviourTree();
                var root = behaviourTree.AddChild<SelectorTask>();
                    var mainTree = root.AddChild<ParallelTask>();
                        var movement = mainTree.AddChild<ParallelTask>();
                            movement.AddChild(new MoveToPointTask(Target));
            return behaviourTree;
        }

        protected override Blackboard BuildBlackboard() {
            var bb = new Blackboard();
            return bb;
        }

        protected override void OnDestoy() {
            base.OnDestoy();
        }

        private void Update() {
            UpdateBT();
        }
    }
}
