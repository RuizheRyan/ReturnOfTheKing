using Photon.Pun;
using Photon.Realtime;
using System;
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
    private Button startButton;
    [SerializeField]
    public Button colorButton;
    [SerializeField]
    private Button _readyButton;
    [SerializeField]
    public GameObject readyInformationForMasterClient;
    [SerializeField]
    private Texture _readyTexture;
    [SerializeField]
    private Texture _unreadyTexture;
    [SerializeField]
    private Texture _someoneNotReadyTexture;
    [SerializeField]
    private Texture _everyoneIsReadyTexture;
    [SerializeField]
    private GameObject _readyInformation;
    [SerializeField]
    private GameObject _playerNumebrInformation;

    private bool _everyOneIsReady = false;
    private bool _localReadyState = false;
    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();

    public enum playerState {playersIsNotEnough,someOneIsUnready,everyIsReady}

    public bool localReadyState
    {
        get
        {
            return _localReadyState;
        }
        set
        {
            _localReadyState = value;
            if (_localReadyState)
            {
                _readyButton.GetComponent<RawImage>().texture = _readyTexture;
            }
            else
            {
                _readyButton.GetComponent<RawImage>().texture = _unreadyTexture;
            }
        }
    }

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
                readyInformationForMasterClient.GetComponent<RawImage>().texture = _everyoneIsReadyTexture;
            }
            else
            {
                _preparePage.inRoomPage.startButton.interactable = false;
                //_everyOneIsReadyText.text = "Unready";
                readyInformationForMasterClient.GetComponent<RawImage>().texture = _someoneNotReadyTexture;
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
        _customProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (PhotonNetwork.IsMasterClient)
        {
            _readyButton.interactable = false;
            //changeAfterTest
            startButton.interactable = true;
            _readyButton.GetComponent<RawImage>().texture = _readyTexture;
            localReadyState = true;
            _customProperties["ReadyState"] = 1;
        }
        else
        {
            _readyButton.interactable = true;
            startButton.interactable = false;
            readyInformationForMasterClient.SetActive(false);
            localReadyState = false;
            _customProperties["ReadyState"] = 0;
        }
        PhotonNetwork.SetPlayerCustomProperties(_customProperties);
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
                if (player.IsMasterClient)
                {
                    block.isSeeker = true;
                }
                else
                {
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
        if (PhotonNetwork.IsMasterClient)
        {
            checkReadyState();
        }
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
            localReadyState = !localReadyState;
            _customProperties["ReadyState"] = Convert.ToInt32(localReadyState);
            PhotonNetwork.SetPlayerCustomProperties(_customProperties);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.All, PhotonNetwork.LocalPlayer, localReadyState);
        }
    }

    private void checkReadyState()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            everyOneIsReady = true;
            for (int i = 0; i < _playerBlockList.Count; i++)
            {
                if (_playerBlockList[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!_playerBlockList[i].readyState)
                    {
                        everyOneIsReady = false;
                        Debug.Log("Someone is unready");
                        break;
                    }
                }
            }                    
        }
        if (_playerBlockList.Count < 3)
        {
            Debug.Log("less than 3");
            photonView.RPC("RPC_ChangeClientInformation", RpcTarget.All, playerState.playersIsNotEnough);
        }
        else
        {
            if (everyOneIsReady)
            {
                Debug.Log("everyone ready");
                photonView.RPC("RPC_ChangeClientInformation", RpcTarget.All, playerState.everyIsReady);
            }
            else
            {
                Debug.Log("someoneUnready");
                photonView.RPC("RPC_ChangeClientInformation", RpcTarget.All, playerState.someOneIsUnready);
            }
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
    private void RPC_ChangeReadyState(Player player, bool playerReadyState)
    {
        int index = _playerBlockList.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _playerBlockList[index].readyState = playerReadyState;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            checkReadyState();
        }
    }

    [PunRPC] void RPC_ChangeClientInformation(playerState state)
    {
        switch (state)
        {
            case playerState.everyIsReady:
                {
                    Debug.Log("set active false");
                    _playerNumebrInformation.SetActive(false);
                    _readyInformation.SetActive(false);
                    Debug.Log(_readyInformation.activeSelf);
                    Debug.Log("finished");
                    break;
                }
            case playerState.playersIsNotEnough:
                {
                    Debug.Log("not enough called");
                    _playerNumebrInformation.SetActive(true);
                    _readyInformation.SetActive(false);
                    break;
                }
            case playerState.someOneIsUnready:
                {
                    Debug.Log("Unready called");
                    _playerNumebrInformation.SetActive(false);
                    _readyInformation.SetActive(true);
                    break;
                }
        }
    }
}
