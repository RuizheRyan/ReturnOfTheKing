using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "V1.0";
    [SerializeField]
    private string _nickName = "TestPlayerName";

    public string GameVersion
    {
        get
        {
            return _gameVersion;
        }
    }

    public string NickName
    {
        get
        {
            int number = Random.Range(0, 9999);
            return _nickName + number.ToString();
        }
    }
}
