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
    private bool _masterClientWinState = false;

    public string GameVersion
    {
        get
        {
            return _gameVersion;
        }
    }

    public bool masterClientWinState
    {
        get
        {
            return _masterClientWinState;
        }
        set
        {
            _masterClientWinState = value;
        }
    }
}
