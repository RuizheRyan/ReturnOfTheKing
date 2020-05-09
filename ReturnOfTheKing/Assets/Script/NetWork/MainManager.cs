using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singleton/MainManger")]
public class MainManager : SingletonScriptableObject<MainManager>
{
    [SerializeField]
    private GameSettings _gameSettings;
    public static GameSettings GameSettings
    {
        get
        {
            return Instance._gameSettings;
        }
    }
}
