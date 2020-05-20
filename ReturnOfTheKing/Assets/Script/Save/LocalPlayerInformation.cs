using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerInformation : MonoBehaviour
{
    //public string playerName;
    //public float redColor;
    //public float greenColor;
    //public float blueColor;
    private bool _getLocalPlayerInformation = false;
    public bool getLocalPlayerInformation
    {
        get
        {
            return _getLocalPlayerInformation;
        }
        set
        {
            _getLocalPlayerInformation = value;
            if (_getLocalPlayerInformation)
            {
                _hostARoomMenu.upDateCreateButtonState();
                _roomBoard.updateJoinButtonState();
                //_customProperties["CustomColor:Red"] = _playersColor.r;
                //_customProperties["CustomColor:Green"] = _playersColor.g;
                //_customProperties["CustomColor:Blue"] = _playersColor.b;
                _customProperties["PlayerName"] = _playerText.text;
                _customProperties["ReadyState"] = 0;
                PhotonNetwork.SetPlayerCustomProperties(_customProperties);                
            }
        }
    }
    [SerializeField]
    private InputField _playerText;
    [SerializeField]
    private Button _colorButton;
    [SerializeField]
    private HostARoomMenu _hostARoomMenu;
    [SerializeField]
    private RoomBoard _roomBoard;
    [SerializeField]

    private Color _playersColor;

    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
    public void getARandomColor()
    {
        float r = Random.Range(0, 1.0f);
        float g = Random.Range(0, 1.0f);
        float b = Random.Range(0, 1.0f);
        _playersColor = new Color(r, g, b);
        setPlayerColor(_playersColor);
    }

    public void Awake()
    {
        Debug.Log("CalledAwake");
        _playerText.text = checkSavedName();
        checkSavedColor();     
        checkSelfInformation();
    }
    public string checkSavedName()
    {
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            return null;
        }
        else
        {
            return PlayerPrefs.GetString("PlayerName");           
        }
    }

    public void checkSavedColor()
    {
        if (!PlayerPrefs.HasKey("RedColor") || !PlayerPrefs.HasKey("GreenColor")|| !PlayerPrefs.HasKey("BlueColor"))
        {
            _playersColor = Color.white;
        }
        else
        {
            _playersColor = new Color(PlayerPrefs.GetFloat("RedColor"), PlayerPrefs.GetFloat("GreenColor"), PlayerPrefs.GetFloat("BlueColor"));
        }
        _colorButton.GetComponent<Image>().color = _playersColor;
    }

    public void setPlayerName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
        checkSelfInformation();
    }

    public void setPlayerColor(Color color)
    {
        
        PlayerPrefs.SetFloat("RedColor", color.r);
        PlayerPrefs.SetFloat("GreenColor", color.g);
        PlayerPrefs.SetFloat("BlueColor", color.b);
        PlayerPrefs.Save();
        _colorButton.GetComponent<Image>().color = color;
        checkSelfInformation();
    }

    private void checkSelfInformation()
    {
        //bool validName = false;
        if (_playerText.text != null && _playerText.text != "")
        {
            getLocalPlayerInformation = true;
        }
        else
        {
            getLocalPlayerInformation = false;
        }
        //if (validName && _playersColor != Color.white)
        //{
        //    getLocalPlayerInformation = true;
        //}
        //else
        //{
        //    getLocalPlayerInformation = false;
        //}
    }

}
