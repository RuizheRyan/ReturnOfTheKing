using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    Boss boss;
    Renderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<Renderer>();
        boss = GetComponentInParent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        mesh.enabled = boss.isAvailable ? true : false;
    }
}
