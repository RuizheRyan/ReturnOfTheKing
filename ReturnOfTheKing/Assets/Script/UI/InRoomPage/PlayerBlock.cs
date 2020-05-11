using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlock : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    public Player Player
    {
        get;
        private set;
    }

    public void setPlayerInfo (Player player)
    {
        Player = player;
        _text.text = player.NickName;
    }

}