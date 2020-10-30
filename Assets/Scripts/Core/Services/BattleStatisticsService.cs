using Character.Health;
using Game.Match;
using KlimLib.SignalBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;

namespace Core.Services.Game {
    public class BattleStatisticsService : ILoadableService, IUnloadableService {

        [Dependency]
        private readonly SignalBus _SignalBus;
        [Dependency]
        private readonly MatchData _MatchData;

        private Dictionary<byte, List<KillData>> _KillsDict;
        public IReadOnlyDictionary<byte, List<KillData>> KillsDict => _KillsDict;

        public void Load() {
            _SignalBus.Subscribe<CharacterDeathSignal>(OnCharacterDeath, this);
            _SignalBus.Subscribe<MatchStartSignal>(OnMatchStart, this);
        }

        public void Unload() {
            _SignalBus.UnSubscribeFromAll(this);
        }

        private void OnCharacterDeath(CharacterDeathSignal signal) {
            var dmg = signal.Damage;
            if (dmg.InstigatorId == null)
                return;
            if (dmg.Receiver.OwnerId == null)
                return;
            var instigator = dmg.InstigatorId.Value;
            if (!_KillsDict.ContainsKey(instigator))
                _KillsDict.Add(instigator, new List<KillData>());
            _KillsDict[instigator].Add(new KillData(instigator, dmg.Receiver.OwnerId.Value));
        }

        private void OnMatchStart(MatchStartSignal signal) {
            _KillsDict = new Dictionary<byte, List<KillData>>();
            foreach (var playerData in _MatchData.Players) {
                _KillsDict.Add(playerData.PlayerId, new List<KillData>());
            }
        }
    }
}

public struct KillData {
    public byte KillerPlayerId;
    public byte KilledPlayerId;

    public KillData(byte killerPlayerId, byte killedPlayerId) {
        this.KillerPlayerId = killerPlayerId;
        this.KilledPlayerId = killedPlayerId;
    }
}
