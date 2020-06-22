namespace Character.Health {
    public struct CharacterDeathSignal {
        public Damage Damage;

        public CharacterDeathSignal(Damage dmg) {
            this.Damage = dmg;
        }
    }
}
