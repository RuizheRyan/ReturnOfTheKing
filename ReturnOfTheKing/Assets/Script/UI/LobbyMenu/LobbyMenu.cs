using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private HostARoomMenu _hostARoomMenu;
    [SerializeField]
    private RoomBoard _roomBoard;

    private PreparePage _preparePage;
    
    public void firstInitialize(PreparePage page)
    {
        _preparePage = page;
        _hostARoomMenu.firstInitialize(page);
        _roomBoard.FirstInitialize(page);
    }
}
