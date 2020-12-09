using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UI.Game;
using UnityDI;
using UnityEngine;

namespace UI {
    public class UIManager : MonoBehaviour {
        private Dictionary<Type, IUIPanel> _PanelDict = new Dictionary<Type, IUIPanel>();
        private List<IUIPanel> _Panels = new List<IUIPanel>();

        private void Awake() {
            CollectPanels();
            DeactivateAll();
            //SetActivePanel<MainPanel>();
        }

        public void DeactivateAll() {
            _Panels.ForEach(_ => _.Deactivate());
        }

        private void CollectPanels() {
            _Panels = GetComponentsInChildren<IUIPanel>(true).ToList();
            _PanelDict = _Panels.ToDictionary(_ => _.GetType());
        }

        public T SetActivePanel<T>() where T : IUIPanel {
            var type = typeof(T);
            if (!_PanelDict.ContainsKey(type))
                Debug.LogError($"UI Manager has no panel type of {type}");
            var panel = (T) _PanelDict[type];
            DeactivateAll();
            panel.Activate();
            panel.Setup();
            return panel;
        }

        private void OnDestroy() {
            ContainerHolder.Container.Unregister<UIManager>();
        }
    }
}