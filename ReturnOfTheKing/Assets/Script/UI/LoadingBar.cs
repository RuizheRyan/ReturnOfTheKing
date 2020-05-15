using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviourPun
{
    public float maxHP = 100;
    public Vector2 offset;
    Camera mainCam;
    [SerializeField]
    BossAction boss;
    [SerializeField]
    Image bar;


    void Start()
    {
        mainCam = Camera.main;
        if (!PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
        GetComponentInParent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponentInParent<Canvas>().enabled = boss.switchTimer > 0 ? true : false;
        }
        if (transform.parent.parent != null)
        {
            transform.parent.SetParent(null);
        }
        Vector2 screenPos = mainCam.WorldToScreenPoint(boss.transform.position - Vector3.forward * 3);
        transform.position = screenPos + offset;
        bar.fillAmount = boss.switchTimer / boss.switchCoolDown;
    }
}
