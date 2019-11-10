using UnityEngine;

public static class Layers {

    private static class Names {
        public const string Default = "Bone";
        public const string Ground = "Ground";
    }

    public static class Masks {

        public static int Default { get; private set; }
        public static int Walkable { get; private set; }

        static Masks() {
            Default = LayerMask.GetMask(Names.Default);
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