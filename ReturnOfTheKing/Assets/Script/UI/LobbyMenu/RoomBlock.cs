using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class RoomBlock : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private RoomBoard _roomBoard;
    public RoomInfo roomInformation { get; private set; }

    public void setRoomInfo (RoomInfo roomInfo)
    {
        roomInformation = roomInfo;
        _text.text = roomInfo.Name;
    }

    public void roomSelected()
    {
        _roomBoard.selectedRoomName = _text.text;
        this.GetComponent<Button>().interactable = false;
    }

}