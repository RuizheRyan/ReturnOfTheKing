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
    private Button _startButton;

    private PreparePage _preparePage;

    private List<PlayerBlock> _playerBlockList = new List<PlayerBlock>();


    public void firstInitialize(PreparePage page)
    {
        _preparePage = page;
    }


    public override void OnEnable()
    {
        base.OnEnable();
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

}
