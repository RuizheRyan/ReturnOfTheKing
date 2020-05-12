using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantialteTest : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    private void Awake()
    {
        MainManager.NetworkInstantiate(_prefab, transform.position, Quaternion.identity);
    }
}
