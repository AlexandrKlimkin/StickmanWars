namespace Character.Health {
    public struct CharacterDeathSignal {
        public byte PlayerId;
        public string CharacterId;

        public CharacterDeathSignal(byte playerId, string characterId) {
            this.PlayerId = playerId;
            this.CharacterId = characterId;
        }
    }
}
