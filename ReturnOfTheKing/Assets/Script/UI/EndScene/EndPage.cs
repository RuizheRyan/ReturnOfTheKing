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
    private Texture _textureOfPlayersWin;
    [SerializeField]
    private Texture _textureOfPlayersLose;
    [SerializeField]
    private Texture _textureOfMonsterWin;
    [SerializeField]
    private Texture _textureOfMonsterLose;
    [SerializeField]
    private RawImage _backGround;

    private float endingTime = 0;

    public void Awake()
    {
        endingTime = 0;
        if (_gameSettings.playerWin)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _backGround.texture = _textureOfMonsterLose;
            }
            else
            {
                _backGround.texture = _textureOfPlayersWin;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _backGround.texture = _textureOfMonsterWin;
            }
            else
            {
                _backGround.texture = _textureOfPlayersLose;
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
            PhotonNetwork.LeaveRoom();
        }
    }
}
