using UnityEngine;

public static class Layers {

    public static class Names {
        public const string Bone = "Bone";
        public const string Box = "Box";
        public const string Ground = "Ground";
        public const string Platform = "Platform";
        public const string Damageable = "Damageable";
        public const string Character = "Character";
        public const string Abyss = "Abyss";
        public const string FallingDownObject = "FallingDownObject";
    }

    public static class Masks {

        public static int Bone { get; private set; }
        public static int Walkable { get; private set; }
        public static int Damageable { get; private set; }
        public static int NoCharacter { get; private set; }
        public static int Character { get; private set; }

        static Masks() {
            Bone = LayerMask.GetMask(Names.Bone);
            Damageable = LayerMask.GetMask(Names.Bone, Names.Damageable);
            Walkable = LayerMask.GetMask(Names.Ground, Names.Platform, Names.Box);
            NoCharacter = CreateLayerMask(true, LayerMask.NameToLayer(Names.Character));
            Character = LayerMask.GetMask(Names.Character);
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