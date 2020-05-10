using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

namespace InputSystem {
    class InputConfig : SingletonScriptableObject<InputConfig> {
        [SerializeField]
        private List<InputKit> _InputKits;
        private Dictionary<int, InputKit> _InputKitsDict;
        private bool _Initialized;

        public IReadOnlyList<InputKit> InputKits => _InputKits;
        public IReadOnlyDictionary<int, InputKit> InputKitsDict => _InputKitsDict;

        private void OnEnable() {
            Initialize(true);
        }

        private void Initialize(bool force = false) {
            if (_Initialized && !force)
                return;
            _Initialized = true;
            _InputKitsDict = _InputKits.ToDictionary(_ => _.Id);
        }

        public InputKit GetSettings(int id) {
            Initialize();
            return !_InputKitsDict.ContainsKey(id) ? null : _InputKitsDict[id];
        }
    }
}
