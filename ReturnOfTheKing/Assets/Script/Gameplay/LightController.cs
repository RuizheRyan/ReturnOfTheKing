using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    Boss boss;
    BossAction bossAction;
    Renderer mesh;
    [SerializeField]
    Light spotLit;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<Renderer>();
        boss = GetComponentInParent<Boss>();
        bossAction = GetComponentInParent<BossAction>();
    }

    // Update is called once per frame
    void Update()
    {
        spotLit.enabled = boss.isAvailable || bossAction.isSwitched ? true : false;
        mesh.enabled = boss.isAvailable || bossAction.isSwitched ? true : false;
        if (bossAction.isSwitched)
        {
            var i = Mathf.Sin(Time.time * 50);
            i = i / 2 + 0.5f;
            mesh.material.SetFloat("_AlphaCutoff", Mathf.Max(i - bossAction.delayTimer / bossAction.delay - 0.2f, 0.01f));
            spotLit.intensity = Mathf.Min(i + bossAction.delayTimer / bossAction.delay + 0.2f, 1) * 20;
        }
        else
        {
            spotLit.intensity = 20;
            mesh.material.SetFloat("_AlphaCutoff", 0.01f);
        }
    }
}
