using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CamraController : MonoBehaviourPun
{
    public bool isBossCam;
    [SerializeField]
    Transform player;
    [SerializeField]
    Camera mainCam;
    Vector3 lastMousePos;
    private float minSize = 25f;
    private float maxSize = 35f;
    float activeDelay = 1.75f;
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
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < maxSize)
            {
                mainCam.orthographicSize += 0.5f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > minSize)
            {
                mainCam.orthographicSize -= 0.5f;
            }
            if (Input.GetMouseButton(2))
            {
                if (lastMousePos != Vector3.zero)
                {
                    Vector3 offset = (lastMousePos - Input.mousePosition) * 0.1f;
                    transform.position += new Vector3(offset.x, offset.y, 0);
                }
            }
            if (Input.GetMouseButtonUp(2))
            {
                lastMousePos = Vector3.zero;
            }
            lastMousePos = Input.mousePosition;
        }
        else
        {
            if(activeDelay >= 0)
            {
                activeDelay -= Time.deltaTime;
                mainCam.orthographicSize = Mathf.Lerp(15f, 35, activeDelay / 1.75f);
            }
            transform.position = player.position;
        }
    }
}
