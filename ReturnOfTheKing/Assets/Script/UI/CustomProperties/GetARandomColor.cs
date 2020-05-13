using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetARandomColor : MonoBehaviour
{
    private Color _playersColor;
    public Color playersColor
    {
        get
        {
            return _playersColor;
        }
        set
        {
            _playersColor = value;
            //Color newcolor = this.GetComponent<Image>().color;
            //newcolor = _playersColor;
            this.GetComponent<Image>().color = _playersColor;
            _customProperties["CustomColor:Red"] = _playersColor.r;
            _customProperties["CustomColor:Green"] = _playersColor.g;
            _customProperties["CustomColor:Blue"] = _playersColor.b;
            PhotonNetwork.SetPlayerCustomProperties(_customProperties);
            _playerInformation.setPlayerColor(_playersColor);
        }
    }
    [SerializeField]
    private LocalPlayerInformation _playerInformation;

    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
    public void getARandomColor()
    {
        float r = Random.Range(0, 1.0f);
        float g = Random.Range(0, 1.0f);
        float b = Random.Range(0, 1.0f);
        playersColor = new Color(r, g, b);
    } 
}
