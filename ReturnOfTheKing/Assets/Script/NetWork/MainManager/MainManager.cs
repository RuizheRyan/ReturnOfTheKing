using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Singleton/MainManger")]
public class MainManager : SingletonScriptableObject<MainManager>
{
    [SerializeField]
    private GameSettings _gameSettings;
    public static GameSettings GameSettings
    {
        get
        {
            return Instance._gameSettings;
        }
    }
    [SerializeField]
    private List<NetworkPrefabs> _networkPrefabs = new List<NetworkPrefabs>();

    public static GameObject NetworkInstantiate(GameObject obj, Vector3 positon, Quaternion rotation)
    {
        foreach(NetworkPrefabs networkPrefab in Instance._networkPrefabs)
        {
            if(networkPrefab.Prefab == obj)
            {
                if (networkPrefab.Path != string.Empty)
                {
                    GameObject result = PhotonNetwork.Instantiate(networkPrefab.Path, positon, rotation);
                    return result;
                }
                else
                {
                    Debug.Log("Empty path for gameobject:" + networkPrefab.Prefab);
                    return null;
                }
            }
        }
        return null;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void PopulateNetworkPrefab()
    {
#if UNITY_EDITOR
        Instance._networkPrefabs.Clear();
        GameObject[] results = Resources.LoadAll<GameObject>("");
        for(int i = 0; i < results.Length; i++)
        {
            if (results[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(results[i]);
                Instance._networkPrefabs.Add(new NetworkPrefabs(results[i], path));
            }
        }
#endif    
    }
}
