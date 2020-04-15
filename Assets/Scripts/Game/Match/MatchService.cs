using System.Collections.Generic;
using Core.Services;
using Tools.Services;
using UnityDI;

namespace Game.Match {
    public class MatchService : ILoadableService, IUnloadableService {

        private MatchData _MatchData;

        public const int MaxPlayerCount = 4;

        public void Load() {
            CreateNewMatch();
        }

        private void CreateNewMatch() {
            _MatchData = new MatchData(new List<PlayerData>());
            ContainerHolder.Container.RegisterInstance(_MatchData);
        }

        public void Unload() {

        }

        public void AddPlayer(PlayerData player) {

        }
    }
}
