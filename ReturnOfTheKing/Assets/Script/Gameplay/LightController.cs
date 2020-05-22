using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicLight2D;

public class LightController : MonoBehaviour
{
    Boss boss;
    BossAction bossAction;
    [SerializeField]
    Renderer mesh;
    DynamicLight dl;
    [SerializeField]
    Light spotLit;

    // Start is called before the first frame update
    void Start()
    {
        mesh = mesh == null ? GetComponent<Renderer>() : mesh;
        boss = GetComponentInParent<Boss>();
        bossAction = GetComponentInParent<BossAction>();
        dl = GetComponent<DynamicLight>();
    }

    // Update is called once per frame
    void Update()
    {
        spotLit.enabled = boss.isAvailable || bossAction.isSwitched ? true : false;
        mesh.enabled = boss.isAvailable || bossAction.isSwitched ? true : false;
        if (bossAction.isSwitched)
        {
            var i = Mathf.Sin(Time.time * 50);
            dl.SortOrderID = Mathf.FloorToInt((i - bossAction.delayTimer / bossAction.delay) * 4);
            i = i / 2 + 0.5f;
            //mesh.material.SetFloat("_AlphaCutoff", Mathf.Max(i - bossAction.delayTimer / bossAction.delay - 0.0f, 0.01f));
            spotLit.intensity = Mathf.Min(i + bossAction.delayTimer / bossAction.delay + 0.2f, 1) * 20;
            //print(mesh.material.GetFloat("_AlphaCutoff"));
        }
        else
        {
            dl.SortOrderID = -1;
            spotLit.intensity = 20;
            //mesh.material.SetFloat("_AlphaCutoff", 0.01f);
        }
    }
}
