using Character.Movement;
using Character.Shooting;
using Tools.BehaviourTree;
using UnityDI;

namespace Game.AI {
    public abstract class CharacterBehaviour : BehaviourTreeExecutor {

        public CharacterUnit CharacterUnit;
        public MovementController MovementController;
        public WeaponController WeaponController;

        protected override void Initialize() {
            ContainerHolder.Container.BuildUp(GetType(), this);
            CharacterUnit = GetComponent<CharacterUnit>();
            MovementController = CharacterUnit.MovementController;
            WeaponController = CharacterUnit.WeaponController;
        }

        protected virtual void OnDestoy() {

        }
    }
}
