using Assets.Scripts.Tools;
using Character.Movement;
using Character.Shooting;
using PlatformerPathfinder2D;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.AI {
    [ExecuteInEditMode]
    public class BotBehaviour : CharacterBehaviour {

        public bool FindPath;
        public Transform Target;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F)) {
                var pathFinder = FindObjectOfType<PlatformerPathfinder>();
                pathFinder.FindPath(transform.position.ToVector2(), Target.position.ToVector2());
                var path = pathFinder.GetPath();
                Debug.LogError(path.Count);
            }
        }

        protected override BehaviourTree BuildBehaviourTree() {
            var behaviourTree = new BehaviourTree();
                var root = behaviourTree.AddChild<SelectorTask>();
            return behaviourTree;
        }

        protected override Blackboard BuildBlackboard() {
            var bb = new Blackboard();
            return bb;
        }

        protected override void OnDestoy() {
            base.OnDestoy();
        }
    }
}
