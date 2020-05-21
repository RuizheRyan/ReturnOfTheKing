using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPage : MonoBehaviour
{
    // Start is called before the first frame update
    public void enterLobby()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (PhotonNetwork.InRoom)
        {
            gameObject.SetActive(false);
        }
    }
}
