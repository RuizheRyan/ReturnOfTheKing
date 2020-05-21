using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HelpBar : MonoBehaviourPun
{
    GameManager gm;
    [SerializeField]
    CharacterController playerController;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Image bar;
    [SerializeField]
    Camera localCam;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        canvas = canvas == null ? GetComponentInParent<Canvas>() : canvas;
        playerController = playerController == null ? GetComponentInParent<CharacterController>() : playerController;
        transform.parent.SetParent(null);
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.deadPlayer != null && !playerController.Dead)
        {
            canvas.enabled = true;
            Vector2 pos = localCam.WorldToScreenPoint(gm.deadPlayer.transform.position);
            pos.x = Mathf.Clamp(pos.x, 50, 1870);
            pos.y = Mathf.Clamp(pos.y, 50, 1030);
            transform.position = pos;
            bar.fillAmount = playerController.rescueTimer / playerController.rescueCoolDown;
        }
        else
        {
            canvas.enabled = false;
        }
    }
}
