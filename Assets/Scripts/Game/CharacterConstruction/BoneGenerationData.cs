using System;
using UnityEngine;
using Character.MuscleSystem;

namespace CharacterConstruction
{
    [Serializable]
    public class BoneGenerationData
    {
        public MuscleType MuscleType;
        public Vector2 WidthRandom;
        public Vector2 HeightRandom;
        public Vector2 ScaleXRandom;
        public Vector2 ScaleYRandom;
    }
}