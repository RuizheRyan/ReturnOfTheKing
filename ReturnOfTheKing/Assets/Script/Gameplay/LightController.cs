using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    Boss boss;
    Renderer mesh;
    [SerializeField]
    Light spotLit;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<Renderer>();
        boss = GetComponentInParent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        spotLit.enabled = boss.isAvailable ? true : false;
        mesh.enabled = boss.isAvailable ? true : false;
    }
}
