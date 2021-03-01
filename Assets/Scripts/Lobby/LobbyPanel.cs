using Core.Services.Photon;
using Photon.Pun;
using System;
using UnityDI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby {
    public class LobbyPanel : MonoBehaviour {
        [Dependency]
        private readonly PhotonLobbyService _PhotonLobbyService;

        public Button CreateRoomButton;
        public Button JoinRoomButton;
        public Button StartButton;

        public Text PlayerInRoomText;

        protected virtual void Awake() {
            ContainerHolder.Container.BuildUp(this);
            CreateRoomButton.onClick.AddListener(CreateRoom);
            JoinRoomButton.onClick.AddListener(JoinRoom);
            StartButton.onClick.AddListener(StartMatch);
        }

        private void Update() {
            PlayerInRoomText.gameObject.SetActive(PhotonNetwork.InRoom);
            if (PhotonNetwork.InRoom) {
                PlayerInRoomText.text = $"Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}";
            }
            StartButton.interactable = PhotonNetwork.IsMasterClient;
        }

        private void CreateRoom() {
            _PhotonLobbyService.CreateRoom();
        }

        private void JoinRoom() {
            _PhotonLobbyService.JoinRoom();
        }

        private void StartMatch() {
            _PhotonLobbyService.StartMatch();
        }

        protected virtual void OnDestroy() {
            JoinRoomButton.onClick.RemoveAllListeners();
            CreateRoomButton.onClick.RemoveAllListeners();
            StartButton.onClick.RemoveAllListeners();
        }

    }
}
