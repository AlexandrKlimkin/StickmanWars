using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.MuscleSystem {

    [Serializable]
    class InertionStopAction : MuscleAction {

        private Muscle _Hip;

        public float InertiaStopForce;

        public override void Initialize(List<Muscle> muscles) {
            _Hip = muscles.FirstOrDefault(_ => _.MuscleType == MuscleType.HipUp);
        }

        public override void UpdateAction(params float[] parameters) {
            if(Mathf.Abs(_Hip.Rigidbody.velocity.x) > 2f) {
                _Hip.Rigidbody.AddForce(new Vector2(InertiaStopForce, 0) * -_Hip.Rigidbody.velocity.x * Time.fixedDeltaTime);
            }
        }

    }
}