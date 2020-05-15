using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPage : MonoBehaviour
{
    [SerializeField]
    private GameSettings _gameSettings;
    [SerializeField]
    private Texture _textureOfWin;
    [SerializeField]
    private Texture _textureOfLose;
    [SerializeField]
    private RawImage _backGround;

    private float endingTime = 0;

    public void Awake()
    {
        endingTime = 0;
        if (_gameSettings.masterClientWinState)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _backGround.texture = _textureOfWin;
            }
            else
            {
                _backGround.texture = _textureOfLose;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _backGround.texture = _textureOfLose;
            }
            else
            {
                _backGround.texture = _textureOfWin;
            }
        }
    }

    public void Update()
    {
        if(endingTime <= 5)
        {
            endingTime += Time.deltaTime;
        }
        else
        {
            endingTime = 0;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(0);
            }
        }
    }
}
