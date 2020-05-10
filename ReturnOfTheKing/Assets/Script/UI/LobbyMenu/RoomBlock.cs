using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBlock : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    public RoomInfo roomInformation { get; private set; }

    public void setRoomInfo (RoomInfo roomInfo)
    {
        roomInformation = roomInfo;
        _text.text = roomInfo.Name;
    }

}
