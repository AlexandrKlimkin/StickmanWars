using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

namespace UI {
    public class UIPanel : MonoBehaviour, IUIPanel {
        public bool Active { get; private set; }
        private bool _Initialized;

        protected void Initialize() {
            if (_Initialized)
                return;
            _Initialized = true;
            Active = gameObject.activeSelf;
        }

        void IUIPanel.Activate() {
            Initialize();
            if (Active)
                return;
            Active = true;
            gameObject.SetActive(true);
            Setup();
        }

        void IUIPanel.Deactivate() {
            Initialize();
            if (!Active)
                return;
            Active = false;
            gameObject.SetActive(false);
        }

        public virtual void Setup() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }
    }

    public interface IUIPanel {
        void Activate();
        void Deactivate();
        void Setup(); //Todo: SetupDataContainer
    }
}
