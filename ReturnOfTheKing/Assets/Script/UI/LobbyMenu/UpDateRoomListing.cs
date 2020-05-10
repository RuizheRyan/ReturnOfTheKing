using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDateRoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomBlock _roomBlock;

    [SerializeField]
    private Transform _content;

    private List<RoomBlock> _roomBlockList = new List<RoomBlock>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _roomBlockList.FindIndex(x => x.roomInformation.Name == info.Name);
                if(index != -1)
                {
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

}
