using Core.Services;
using Core.Services.SceneManagement;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Tools.Unity;
using UnityDI;
using UnityEngine;

namespace Core.Services.Photon {
    public class PhotonLobbyService : IConnectionCallbacks, IMatchmakingCallbacks, ILoadableService, IUnloadableService, IOnEventCallback {
        [Dependency]
        private readonly UnityEventProvider _EventProvider;
        [Dependency]
        private readonly SceneManagerService _SceneManagerService;

        private const string _GameVersion = "1";
        private const byte _StartMatchEventCode = 1;


        public void Load() {
            PhotonNetwork.AddCallbackTarget(this);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = _GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Unload() {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void CreateRoom() {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
        }

        public void JoinRoom() {
            PhotonNetwork.JoinRandomRoom();
        }

        public void StartMatch() {
            PhotonNetwork.RaiseEvent(_StartMatchEventCode, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent) {
            if(photonEvent.Code == _StartMatchEventCode) {
                _SceneManagerService.LoadScene(SceneType.MapSelection);
            }
        }

        #region IConnectionCallbacks
        public void OnConnected() { }

        public void OnConnectedToMaster() {
            Debug.Log("OnConnectedToMaster was called");
        }

        public void OnDisconnected(DisconnectCause cause) {
            Debug.LogWarningFormat($"OnDisconnected was called, reason - {cause}");
        }

        public void OnRegionListReceived(RegionHandler regionHandler) { }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

        public void OnCustomAuthenticationFailed(string debugMessage) { }
        #endregion

        #region IMatchmakingCallbacks
        public void OnFriendListUpdate(List<FriendInfo> friendList) { }

        public void OnCreatedRoom() { }

        public void OnCreateRoomFailed(short returnCode, string message) { }

        public void OnJoinedRoom() {
            Debug.Log($"OnJoinedRoom was called");
        }

        public void OnJoinRoomFailed(short returnCode, string message) {
            Debug.Log($"OnJoinRoomFailed was called");
        }

        public void OnJoinRandomFailed(short returnCode, string message) { }

        public void OnLeftRoom() { }
        #endregion
    }
}