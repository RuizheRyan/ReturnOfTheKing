using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoom : MonoBehaviour
{
    private PreparePage _preparePage;

    public void firstInitialize(PreparePage page)
    {
        _preparePage = page;
    }

    public void leaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        _preparePage.inRoomPage.hideSelf();
        _preparePage.lobbyMenu.localPlayerInformation.Awake();
    }
}
