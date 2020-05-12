using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetARandomColor : MonoBehaviour
{
    private Color _playersColor;
    //[SerializeField]
    //private PlayerBoard _playBoard;

    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
    public void getARandomColor()
    {
        _playersColor.r = Random.Range(0, 1.0f);
        _playersColor.g = Random.Range(0, 1.0f);
        _playersColor.b = Random.Range(0, 1.0f);
        _playersColor.a = 1;
        Color newcolor = this.GetComponent<Image>().color;
        newcolor = _playersColor;
        this.GetComponent<Image>().color = newcolor;

        _customProperties["CustomColor:Red"] = _playersColor.r;
        _customProperties["CustomColor:Green"] = _playersColor.g;
        _customProperties["CustomColor:Blue"] = _playersColor.b;
        PhotonNetwork.SetPlayerCustomProperties(_customProperties);
    } 
}
