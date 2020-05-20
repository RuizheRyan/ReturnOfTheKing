﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CamraController : MonoBehaviourPun
{
    public bool isBossCam;
    [SerializeField]
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        if (isBossCam && !PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
        else if(!isBossCam)
        {
            player = player == null ? transform.parent : player;
            if (!player.GetComponent<PhotonView>().IsMine)
            {
                gameObject.SetActive(false);
            }
            transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isBossCam)
        {
            return;
        }
        transform.position = player.position;
    }
}
