using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetARandomColor : MonoBehaviour
{
    public Color playersColor;

    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
    public void getARandomColor()
    {
        playersColor.r = Random.Range(0, 1.0f);
        playersColor.g = Random.Range(0, 1.0f);
        playersColor.b = Random.Range(0, 1.0f);
        playersColor.a = 1;
        Color newcolor = this.GetComponent<Image>().color;
        newcolor = playersColor;
        this.GetComponent<Image>().color = newcolor;

        _customProperties["CustomColor:Red"] = playersColor.r;
        _customProperties["CustomColor:Green"] = playersColor.g;
        _customProperties["CustomColor:Blue"] = playersColor.b;
        PhotonNetwork.LocalPlayer.CustomProperties = _customProperties;
    } 
}
