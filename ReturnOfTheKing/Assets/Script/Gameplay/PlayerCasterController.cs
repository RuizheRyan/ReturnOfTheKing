using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasterController : MonoBehaviour
{
    public GameObject player;
    GameManager gm;
    Transform currentTowerTrans;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            var playerPos = player.transform.position/* + (player.transform.position - gm.CurrentTower.transform.position).normalized * 1.5f*/;
            playerPos.z = 0;
            transform.position = playerPos;
        }
    }
}
