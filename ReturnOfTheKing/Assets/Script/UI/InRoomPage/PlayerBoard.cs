using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoard: MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerBlock _playerBlock;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Text _readyText;

    private PreparePage _preparePage;
    private bool _everyOneIsready = false;

    private List<PlayerBlock> _playerBlockList = new List<PlayerBlock>();


    public void firstInitialize(PreparePage page)
    {
        _preparePage = page;
    }

    public override void OnEnable()
    {
        base.OnEnable();
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
        _everyOneIsready = state;
        if (_everyOneIsready)
        {
            _readyText.text = "R";
        }
        else
        {
            _readyText.text = "N";
        }
    }

   
    private void getCurrentPlayersInTheRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;
        foreach(KeyValuePair<int,Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
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
                _playerBlock.playerBoard = this;
                if (player.IsMasterClient)
                    _playerBlock.ready = true;
                _playerBlockList.Add(block);
            }
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("PlayersEntered.");
        addPlayerBlockIntoPlayerBoard(newPlayer);
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
            setReadyUp(!_everyOneIsready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient,PhotonNetwork.LocalPlayer,_everyOneIsready);
        }
    }
    
    [PunRPC]
    private void RPC_ChangeReadyState(Player player,bool ready)
    {
        int index = _playerBlockList.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _playerBlockList[index].ready = ready;
        }
    }

    public void checkReadyState()
    {
        if (PhotonNetwork.IsMasterClient) {
            for (int i = 0; i < _playerBlockList.Count; i++)
            {
                if (_playerBlockList[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!_playerBlockList[i].ready)
                    {
                        _everyOneIsready = false;
                        _preparePage.inRoomPage.startButton.interactable = false;
                        return;
                    }
                }
            }
            _everyOneIsready = true;
            _preparePage.inRoomPage.startButton.interactable = true;
        }
    }

}
