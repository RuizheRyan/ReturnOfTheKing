using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "V1.0";
    [SerializeField]
    private bool _playerWin = false;

    public string GameVersion
    {
        get
        {
            return _gameVersion;
        }
    }

    public bool playerWin
    {
        get
        {
            return _playerWin;
        }
        set
        {
            _playerWin = value;
        }
    }
}
