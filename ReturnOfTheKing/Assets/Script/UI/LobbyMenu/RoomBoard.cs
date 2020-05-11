using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class RoomBoard : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomBlock _roomBlock;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Button _joinButton;

    private List<RoomBlock> _roomBlockList = new List<RoomBlock>();


    private string _selectedRoomName = null;
    public string selectedRoomName
    {
        get
        {
            return _selectedRoomName;
        }
        set
        {
            _selectedRoomName = value;
            if (_selectedRoomName == null || _selectedRoomName == "")
            {
                _joinButton.interactable = false;
            }
            else
            {
                _joinButton.interactable = true;
                foreach(RoomBlock roomBlock in _roomBlockList)
                {
                    if (roomBlock.roomInformation.Name != selectedRoomName)
                    {
                        roomBlock.GetComponent<Button>().interactable = true;
                    }
                }
            }
        }
    }

    private PreparePage _preparePage;

    public void FirstInitialize(PreparePage page)
    {
        _preparePage = page;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _roomBlockList.FindIndex(x => x.roomInformation.Name == info.Name);
                if(index != -1)
                {
                    if(info.Name == selectedRoomName)
                    {
                        selectedRoomName = null;
                    }
                    Destroy(_roomBlockList[index].gameObject);
                    _roomBlockList.RemoveAt(index);
                }
            }
            else
            {
                int index = _roomBlockList.FindIndex(x => x.roomInformation.Name == info.Name);
                if (index == -1)
                {
                    RoomBlock block = Instantiate(_roomBlock, _content);
                    if (block != null)
                    {
                        block.setRoomInfo(info);
                        block._roomBoard = this;
                        _roomBlockList.Add(block);
                    }
                }
                else
                {
                    //modify existed block
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        _preparePage.inRoomPage.showSelf();
        _content.destroyChildren();
        _roomBlockList.Clear();
        if (PhotonNetwork.IsMasterClient)
        {
            _preparePage.inRoomPage.startButton.interactable = true;
            _preparePage.inRoomPage.readyButton.interactable = false;
        }
        else
        {
            _preparePage.inRoomPage.startButton.interactable = false;
        }
        _preparePage.inRoomPage.colorButton.image.color = _preparePage.lobbyMenu.colorButton.image.color;
    }

    public void joinSelectedRoom()
    {
        //Debug.Log(selectedRoomName);
        if (selectedRoomName != null || selectedRoomName != "")
        {
            PhotonNetwork.JoinRoom(selectedRoomName);
        }    
    }
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("Entered:" + selectedRoomName);
    //}
}
