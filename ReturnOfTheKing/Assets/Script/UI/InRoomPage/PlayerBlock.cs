using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlock : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Texture _seekerTexture;
    [SerializeField]
    private Texture _hiderTexture;

    private bool _readyState = false;

    private bool _isSeeker = false;
    public bool isSeeker
    {
        get
        {
            return _isSeeker;
        }
        set
        {
            _isSeeker = value;
            if (_isSeeker)
            {
                GetComponent<RawImage>().texture = _seekerTexture;
            }
            else
            {
                GetComponent<RawImage>().texture = _hiderTexture;
            }
        }
    }

    public bool readyState
    {
        get
        {
            return _readyState;
        }
        set
        {
            _readyState = value;
            if (_readyState)
            {
                this.GetComponent<Button>().interactable = true;
            }
            else
            {
                this.GetComponent<Button>().interactable = false;
            }
            //roomPage.checkReadyState();
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
        _text.text = (string)player.CustomProperties["PlayerName"];
        int state = (int)player.CustomProperties["ReadyState"];
        if (state == 0)
        {
            readyState = false;
        }
        else
        {
            readyState = true;
        }
        //setPlayerBlock(player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (targetPlayer != null && targetPlayer == Player)
        {
            setPlayerBlock(targetPlayer);
        }
    }

    private void setPlayerBlock(Player player)
    {
        //float redValue = 1;
        //float greenValue = 1;
        //float blueValue = 1;
        //if (player.CustomProperties.ContainsKey("CustomColor:Red"))
        //    redValue = (float)player.CustomProperties["CustomColor:Red"];
        //if (player.CustomProperties.ContainsKey("CustomColor:Green"))
        //    greenValue = (float)player.CustomProperties["CustomColor:Green"];
        //if (player.CustomProperties.ContainsKey("CustomColor:Blue"))
        //    blueValue = (float)player.CustomProperties["CustomColor:Blue"];
        //this.GetComponent<RawImage>().color = new Color(redValue, greenValue, blueValue, 1);
        _text.text = (string)player.CustomProperties["PlayerName"];
    }
}