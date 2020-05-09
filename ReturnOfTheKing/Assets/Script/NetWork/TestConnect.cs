using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        print("connecting...");
        PhotonNetwork.GameVersion = "V1.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("connected");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("Disconnect for" + cause.ToString());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
