using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparePage : MonoBehaviour
{
    [SerializeField]
    private LobbyMenu _lobbyMenu;
    public LobbyMenu lobbyMenu
    {
        get
        {
            return _lobbyMenu;
        }
    }
    [SerializeField]
    private InRoomPage _inRoomPage;
    public InRoomPage inRoomPage
    {
        get
        {
            return _inRoomPage;
        }
    }

    private void Awake()
    {
        firstInitialize();
    }

    private void firstInitialize()
    {
        lobbyMenu.firstInitialize(this);
        inRoomPage.firstInitialize(this);
        if (PhotonNetwork.InRoom)
        {
            _inRoomPage.gameObject.SetActive(true);
        }
    }

}
