using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourPun
{
    public float maxHP = 100;
    public Vector2 offset;
    Camera mainCam;
    [SerializeField]
    CharacterController player;
    [SerializeField]
    Image bar;


    void Start()
    {
        maxHP = player.fullHealth;
        mainCam = Camera.main;
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
        else
        {
            photonView.TransferOwnership(player.photonView.Owner);
            transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = mainCam.WorldToScreenPoint(player.transform.position - Vector3.forward * 2);
        transform.position = screenPos + offset;
        bar.fillAmount = player.currentHealth / maxHP;
    }
}
