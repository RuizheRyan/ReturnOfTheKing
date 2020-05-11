﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlock : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Text _readyText;

    private bool _ready = false;
    public PlayerBoard playerBoard;

    public bool ready
    {
        get
        {
            return _ready;
        }
        set
        {
            _ready = value;
            if (_ready)
            {
                _readyText.text = "Ready";
            }
            else
            {
                _readyText.text = "Unready";
            }
            playerBoard.checkReadyState();
        }
    } 
    public Player Player
    {
        get;
        private set;
    }

    public void setPlayerInfo (Player player)
    {
        Player = player;
        setPlayerBlock(player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if(targetPlayer != null && targetPlayer == Player)
        {
            setPlayerBlock(targetPlayer);
        }
    }

    private void setPlayerBlock(Player player)
    {
        float redValue = 0;
        float greenValue = 0;
        float blueValue = 0;
        if (player.CustomProperties.ContainsKey("CustomColor:Red"))
            redValue = (float)player.CustomProperties["CustomColor:Red"];
        if (player.CustomProperties.ContainsKey("CustomColor:Green"))
            greenValue = (float)player.CustomProperties["CustomColor:Green"];
        if (player.CustomProperties.ContainsKey("CustomColor:Blue"))
            blueValue = (float)player.CustomProperties["CustomColor:Blue"];
        this.GetComponent<RawImage>().color = new Color(redValue, greenValue, blueValue, 1);
        _text.text = player.NickName;
    }
}