using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourPun
{
    public float maxHP = 100;
    //Camera mainCam;
    [SerializeField]
    CharacterController player;
    [SerializeField]
    Image bar;
    Canvas canvas;

    private void Awake()
    {
        if (transform.parent.parent != null)
        {
            transform.parent.SetParent(null);
        }
    }

    void Start()
    {
        maxHP = player.fullHealth;
        //mainCam = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        if (transform.parent.parent != null)
        {
            transform.parent.SetParent(null);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            canvas.enabled = false;
        }
        else
        {
            GetComponentInParent<PhotonView>().TransferOwnership(player.photonView.Owner);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            canvas.enabled = player._isDetected;
        }
        //Vector2 screenPos = mainCam.WorldToScreenPoint(player.transform.position - Vector3.forward * 3);
        //transform.position = screenPos + offset;
        canvas.transform.position = player.transform.position + Vector3.forward * -1.5f;
        bar.fillAmount = player.currentHealth / maxHP;
    }
}
