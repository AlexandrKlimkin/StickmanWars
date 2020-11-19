﻿using System;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    [Serializable]
    public class VanMoveParameters {
        public Vector2 Velocity;
        public Transform StartGravityTransform;
        public Transform StartRotationTransform;
        public float GravityAcceleration;
        public AnimationCurve FallingRotationCurve;
        public float MaxAngularSpeed;
        public float RotationTime;
    }
}