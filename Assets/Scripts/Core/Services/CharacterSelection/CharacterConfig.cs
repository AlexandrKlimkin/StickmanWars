using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

namespace Core.Services.MapSelection {
    public class CharacterConfig : SingletonScriptableObject<CharacterConfig> {
        [SerializeField]
        private List<CharacterData> _Characters;

        public IReadOnlyList<CharacterData> Characters => _Characters;

        private Dictionary<string, CharacterData> _CharactersDict;
        private bool _Initialized;


        private void OnEnable() {
            Initialize(true);
        }

        private void Initialize(bool force = false) {
            if (_Initialized && !force)
                return;
            _Initialized = true;
            _CharactersDict = _Characters.ToDictionary(_ => _.Name);
        }

        public CharacterData GetCharacterData(string name) {
            Initialize();
            return !_CharactersDict.ContainsKey(name) ? null : _CharactersDict[name];
        }
    }

    [Serializable]
    public class CharacterData {
        public string Name;
        public string AvatarPath;
    }
}
