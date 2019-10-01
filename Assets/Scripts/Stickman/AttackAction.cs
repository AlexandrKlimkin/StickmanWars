using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MuscleSystem {
    [Serializable]
    public class AttackAction : MuscleAction {

        private List<Muscle> _ArmUp;
        private List<Muscle> _ArmDown;

        public override void Initialize(List<Muscle> muscles) {
            base.Initialize(muscles);
            //_ArmUp = muscles
        }

        public override void UpdateAction(params float[] parameters) {
            
        }

    }
}
