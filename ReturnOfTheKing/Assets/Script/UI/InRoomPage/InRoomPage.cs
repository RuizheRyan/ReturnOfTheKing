using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InRoomPage : MonoBehaviour
{
    [SerializeField]
    private PlayerBoard _playerBoard;
    [SerializeField]
    private LeaveRoom _leaveRoomButton;
    [SerializeField]
    public Button startButton;
    [SerializeField]
    public Button colorButton;

    private PreparePage _preparePage;

    public void firstInitialize(PreparePage page)
    {
        _preparePage = page;
        _leaveRoomButton.firstInitialize(page);
        _playerBoard.firstInitialize(page);
    }

    public void showSelf()
    {
        gameObject.SetActive(true);
    }
    public void hideSelf()
    {
        gameObject.SetActive(false);
    }

    public void startGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }
}
