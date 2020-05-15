using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    [SerializeField]
    GameObject _playerPrefab;
    [SerializeField]
    GameObject _monsterPrefab;
    [SerializeField]
    private Vector3 _startPointOfMonster = new Vector3(10, 1, 10);
    [SerializeField]
    private Vector3 _startPointOfPlayer1 = new Vector3(0, 1, 10);
    [SerializeField]
    private Vector3 _startPointOfPlayer2 = new Vector3(20, 1, 10);
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {           
             MainManager.NetworkInstantiate(_monsterPrefab, _startPointOfMonster, Quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                MainManager.NetworkInstantiate(_playerPrefab, _startPointOfPlayer1, Quaternion.identity);
                
            }
            else
            {
                MainManager.NetworkInstantiate(_playerPrefab, _startPointOfPlayer2, Quaternion.identity);
            }
        }
    }
}
