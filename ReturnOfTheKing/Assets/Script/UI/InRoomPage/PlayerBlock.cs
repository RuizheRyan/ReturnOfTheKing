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
        float redValue = 0;
        float greenValue = 0;
        float blueValue = 0;
        if(player.CustomProperties.ContainsKey("CustomColor:Red"))
            redValue = (float)player.CustomProperties["CustomColor:Red"];
        if (player.CustomProperties.ContainsKey("CustomColor:Green"))
            greenValue = (float)player.CustomProperties["CustomColor:Green"];
        if (player.CustomProperties.ContainsKey("CustomColor:Blue"))
            blueValue = (float)player.CustomProperties["CustomColor:Blue"];
        this.GetComponent<RawImage>().color = new Color(redValue,greenValue,blueValue,1);
        _text.text = player.NickName;
    }

}