using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Lobby;

namespace UI.Lobby {
    public class CreateRoomButton : PhotonActionButton {
        protected override void ButtonAction() {
            PhotonLobbyService.CreateRoom();
        }
    }
}