﻿using UnityEngine;

public static class Layers {

    private static class Names {
        public const string Bone = "Bone";
        public const string Ground = "Ground";
    }

    public static class Masks {

        public static int Bone { get; private set; }
        public static int Walkable { get; private set; }
        public static int Damageable { get; private set; }

        static Masks() {
            Bone = LayerMask.GetMask(Names.Bone);
            Damageable = LayerMask.GetMask(Names.Bone);
            Walkable = LayerMask.GetMask(Names.Ground);
        }

        public static int CreateLayerMask(bool aExclude, params int[] aLayers) {
            var v = 0;
            foreach (var L in aLayers)
                v |= 1 << L;
            if (aExclude)
                v = ~v;
            return v;
        }
    }
}