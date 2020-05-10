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

    private string _selectedRoomName = null;
    private List<RoomBlock> _roomBlockList = new List<RoomBlock>();
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
                RoomBlock block = Instantiate(_roomBlock, _content);
                if (block != null)
                {
                    block.setRoomInfo(info);
                    _roomBlockList.Add(block);
                }
            }
        }
    }

    public void joinSelectedRoom()
    {
        PhotonNetwork.JoinRoom(selectedRoomName);
    }
}
