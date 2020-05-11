using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoomPage : MonoBehaviour
{
    [SerializeField]
    private PlayerBoard _playerBoard;
    [SerializeField]
    private LeaveRoom _leaveRoomButton;

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
}
