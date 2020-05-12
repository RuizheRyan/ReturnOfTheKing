using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    [SerializeField]
    GameObject _prefab;

    private void Awake()
    {
        Vector3 startpoint1 = new Vector3(0, 1, 5);
        Vector3 startpoint2 = new Vector3(5, 1, 5);

        MainManager.NetworkInstantiate(_prefab, startpoint1, Quaternion.identity);
    }
}
