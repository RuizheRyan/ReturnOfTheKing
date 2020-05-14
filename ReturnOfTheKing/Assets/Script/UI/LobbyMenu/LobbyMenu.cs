using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private HostARoomMenu _hostARoomMenu;
    [SerializeField]
    private RoomBoard _roomBoard;
    [SerializeField]
    public Button colorButton;
    [SerializeField]
    public LocalPlayerInformation localPlayerInformation;

    private PreparePage _preparePage;
    
    public void firstInitialize(PreparePage page)
    {
        _preparePage = page;
        _hostARoomMenu.firstInitialize(page);
        _roomBoard.FirstInitialize(page);
    }
}
