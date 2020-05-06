using System.Collections.Generic;
using Core.Services;
using Tools.Services;
using UnityDI;

namespace Game.Match {
    public class MatchService : ILoadableService, IUnloadableService {

        private MatchData _MatchData;

        public const int MaxPlayerCount = 4;

        private readonly Dictionary<byte, PlayerData> _PlayersDict = new Dictionary<byte, PlayerData>();

        public void Load() {
            CreateNewMatch();
        }

        public void Unload() {
            ContainerHolder.Container.Unregister<MatchData>();
        }

        public void AddPlayer(PlayerData player) {
            _MatchData.Players.Add(player);
            _PlayersDict.Add(player.PlayerId, player);
        }

        public PlayerData GetPlayerData(byte playerId) {
            return _PlayersDict[playerId];
        }

        private void CreateNewMatch() {
            _MatchData = new MatchData(new List<PlayerData>());
            ContainerHolder.Container.RegisterInstance(_MatchData);
        }
    }
}
