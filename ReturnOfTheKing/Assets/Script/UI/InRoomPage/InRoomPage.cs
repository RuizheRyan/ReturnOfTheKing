using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InRoomPage : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerBlock _playerBlock;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    public Button startButton;
    [SerializeField]
    public Button colorButton;
    [SerializeField]
    private Button _readyButton;
    [SerializeField]
    public GameObject readyInformation;
    [SerializeField]
    private Texture _readyTexture;
    [SerializeField]
    private Texture _unreadyTexture;
    [SerializeField]
    private Texture _someoneNotReadyTexture;
    [SerializeField]
    private Texture _everyoneIsReadyTexture;

    private bool _everyOneIsReady = false;
    private bool _localReadyState = false;

    public bool everyOneIsReady
    {
        get
        {
            return _everyOneIsReady;
        }
        set
        {
            _everyOneIsReady = value;
            if (_everyOneIsReady)
            {
                _preparePage.inRoomPage.startButton.interactable = true;
                //_everyOneIsReadyText.text = "Everyone is ready";
                readyInformation.GetComponent<RawImage>().texture = _everyoneIsReadyTexture;
            }
            else
            {
                _preparePage.inRoomPage.startButton.interactable = false;
                //_everyOneIsReadyText.text = "Unready";
                readyInformation.GetComponent<RawImage>().texture = _someoneNotReadyTexture;
            }
        }
    }

    private List<PlayerBlock> _playerBlockList = new List<PlayerBlock>();

    private void Awake()
    {
        //OnEnable();
    }

    private PreparePage _preparePage;

    public void firstInitialize(PreparePage page)
    {
        _preparePage = page;
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

  
    public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonNetwork.IsMasterClient)
        {
            _readyButton.interactable = false;
            startButton.interactable = false;
            _readyButton.GetComponent<RawImage>().texture = _readyTexture;
        }
        else
        {
            _readyButton.interactable = true;
            startButton.interactable = false;
            readyInformation.SetActive(false);
        }
        setReadyUp(false);
        getCurrentPlayersInTheRoom();

    }


    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _playerBlockList.Count; i++)
        {
            Destroy(_playerBlockList[i].gameObject);
        }
        _playerBlockList.Clear();
    }

    private void setReadyUp(bool state)
    {
        _localReadyState = state;
        if (_localReadyState)
        {
            _readyButton.GetComponent<RawImage>().texture = _readyTexture;
        }
        else
        {
            _readyButton.GetComponent<RawImage>().texture = _unreadyTexture;
        }
    }


    private void getCurrentPlayersInTheRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            addPlayerBlockIntoPlayerBoard(playerInfo.Value);
        }
    }

    public void addPlayerBlockIntoPlayerBoard(Player player)
    {
        int index = _playerBlockList.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _playerBlockList[index].setPlayerInfo(player);
        }
        else
        {
            PlayerBlock block = Instantiate(_playerBlock, _content);
            if (block != null)
            {
                block.setPlayerInfo(player);
                block.roomPage = this;
                if (player.IsMasterClient)
                {
                    block.readyState = true;
                    block.isSeeker = true;
                }
                else
                {
                    block.readyState = false;
                    block.isSeeker = false;
                }
                _playerBlockList.Add(block);
            }
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("PlayersEntered.");
        addPlayerBlockIntoPlayerBoard(newPlayer);
        everyOneIsReady = false;
    }

    public override void OnPlayerLeftRoom(Player leftPlayer)
    {
        int index = _playerBlockList.FindIndex(x => x.Player == leftPlayer);
        if (index != -1)
        {
            Destroy(_playerBlockList[index].gameObject);
            _playerBlockList.RemoveAt(index);
        }
    }

    public void readyUp()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            setReadyUp(!_localReadyState);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, _localReadyState);
        }
    }

    public void checkReadyState()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < _playerBlockList.Count; i++)
            {
                if (_playerBlockList[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!_playerBlockList[i].readyState)
                    {
                        everyOneIsReady = false;
                        Debug.Log("Someone is unready");
                        return;
                    }
                }
            }
            everyOneIsReady = true;
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Client leave the room");
        leaveCurrentRoom();
    }

    public void leaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        hideSelf();
        _preparePage.lobbyMenu.localPlayerInformation.checkSavedColor();
        _preparePage.lobbyMenu.localPlayerInformation.checkSavedName();
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _playerBlockList.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _playerBlockList[index].readyState = ready;
        }
    }
}
