using Game.Match;

namespace Character.Health {
    public struct DamageableDeathSignal {
        public Damage Damage;

        public DamageableDeathSignal(Damage dmg) {
            this.Damage = dmg;
        }
    }
}
