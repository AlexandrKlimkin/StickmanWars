namespace Game.Match {
    public struct MatchDataCreatedSignal {
        public MatchData MatchData;

        public MatchDataCreatedSignal(MatchData matchData) {
            this.MatchData = matchData;
        }
    }
}