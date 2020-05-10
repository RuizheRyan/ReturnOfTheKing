using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostARoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;
    [SerializeField]
    private Button _creatButton;

    public void upDateCreateButtonState()
    {
        if (_roomName.text == null || _roomName.text == "")
        {
            _creatButton.interactable = false;
        }
        else
        {
            _creatButton.interactable = true;
        }
    }

    public void createANewRoom()
    {
        if(!PhotonNetwork.IsConnected || _roomName.text == null || _roomName.text == "")
        {
            return;
        }
        else
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 3;
            PhotonNetwork.CreateRoom(_roomName.text, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("create succeed");

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("create failed:" + message);
    }
}
