using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;
    //public Text roomName
    //{
    //    get
    //    {
    //        return _roomName;
    //    }
    //    set
    //    {
    //        _roomName = value;
    //        if (_roomName == null || _roomName.text == "")
    //        {
    //            this.GetComponent<Button>().interactable = false;
    //        }
    //        else
    //        {
    //            this.GetComponent<Button>().interactable = true;
    //        }
    //    }
    //}

    public void addNewRoomToList()
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
