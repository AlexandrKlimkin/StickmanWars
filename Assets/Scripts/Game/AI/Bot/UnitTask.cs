using Character.Movement;
using Character.Shooting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI {
    public abstract class UnitTask : BuildedTask {

        protected CharacterUnit CharacterUnit { get; private set; }
        protected MovementController MovementController { get; private set; }
        protected WeaponController WeaponController { get; private set; }

        public override void Init() {
            base.Init();
            CharacterUnit = BehaviourTree.Executor.GetComponent<CharacterUnit>();
            MovementController = CharacterUnit?.MovementController;
            WeaponController = CharacterUnit?.WeaponController;
        }
    }
}
