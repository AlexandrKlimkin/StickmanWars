using System.Collections.Generic;
using UnityEngine;

namespace MuscleSystem {
    public abstract class MuscleAction {

        protected List<Muscle> _Muscles;

        //public MuscleAction(List<Muscle> muscles) {
        //    _Muscles = muscles;
        //}
        public virtual void Initialize(List<Muscle> muscles) {
            _Muscles = muscles;
        }

        public abstract void UpdateAction(params float[] parameters);

    }
}
