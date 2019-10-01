using MuscleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleSystem {

    [Serializable]
    public class JumpAction : MuscleAction {
        [Header("Settings")]
        public float JumpForce;

        public override void UpdateAction(params float[] parameters) {
            var jumpForce = parameters[0];
            _Muscles.ForEach(muscle => muscle.AddForce(new Vector2(0, jumpForce * JumpForce)));
        }
    }
}
