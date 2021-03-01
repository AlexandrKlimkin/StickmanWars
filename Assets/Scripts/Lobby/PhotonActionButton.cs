using Core.Services.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby {
    public abstract class PhotonActionButton : MonoBehaviour {
        [Dependency]
        protected readonly PhotonLobbyService PhotonLobbyService;

        protected Button Button;

        protected virtual void Awake() {
            ContainerHolder.Container.BuildUp(this);
            Button = GetComponent<Button>();
            Button.onClick.AddListener(ButtonAction);
        }

        protected virtual void OnDestroy() {
            Button.onClick.RemoveAllListeners();
        }

        protected abstract void ButtonAction();
    }
}