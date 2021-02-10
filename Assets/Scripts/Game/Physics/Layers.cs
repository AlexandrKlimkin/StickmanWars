using UnityEngine;

public static class Layers {

    public static class Names {
        public const string Box = "Box";
        public const string Ground = "Ground";
        public const string Platform = "Platform";
        public const string Damageable = "Damageable";
        public const string Character = "Character";
        public const string Abyss = "Abyss";
        public const string FallingDownObject = "FallingDownObject";
        public const string Weapon = "Weapon";
        public const string MovingPlatform = "MovingPlatform";
        public const string OneWayPlatform = "OneWayPlatform";
        public const string Corpse = "Corpse";
    }

    public static class Masks {

        public static int Bone { get; private set; }
        public static int Walkable { get; private set; }
        public static int Box { get; private set; }
        public static int Damageable { get; private set; }
        public static int NoCharacter { get; private set; }
        public static int Character { get; private set; }
        public static int BotVisionMask { get; private set; }
        public static int Obstacle { get; private set; }
        public static int GroundAndPlatform { get; private set; }
        public static int ForProjectiles { get; private set; }

        static Masks() {
            Damageable = LayerMask.GetMask(Names.Damageable);
            Walkable = LayerMask.GetMask(Names.Ground, Names.Platform, Names.Box, Names.MovingPlatform, Names.OneWayPlatform, Names.Corpse);
            NoCharacter = CreateLayerMask(true, LayerMask.NameToLayer(Names.Character));
            Character = LayerMask.GetMask(Names.Character);
            BotVisionMask = LayerMask.GetMask(Names.Ground, Names.Platform, Names.MovingPlatform);
            GroundAndPlatform = LayerMask.GetMask(Names.Ground, Names.Platform);
            Box = LayerMask.GetMask(Names.Box);
            Obstacle = LayerMask.GetMask(Names.Box, Names.Weapon);
            ForProjectiles = CreateLayerMask(true, LayerMask.NameToLayer(Names.OneWayPlatform));
        }

        public static int CreateLayerMask(bool aExclude, params int[] aLayers) {
            var v = 0;
            foreach (var L in aLayers)
                v |= 1 << L;
            if (aExclude)
                v = ~v;
            return v;
        }

        public static bool LayerInMask(int mask, int layer) {
            return mask == (mask | (1 << layer));
        }
    }
}