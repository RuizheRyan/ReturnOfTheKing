using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkPrefabs 
{
    public GameObject Prefab;
    public string Path;

    public NetworkPrefabs(GameObject obj, string path)
    {
        Prefab = obj;
        Path = modifyPath(path);
    }

    private string modifyPath(string path)
    {
        int extentionLength = System.IO.Path.GetExtension(path).Length;
        int addtiontionLength = 10;
        int startIndex = path.ToLower().IndexOf("resources");
        if (startIndex == -1)
            return string.Empty;
        else
            return path.Substring(startIndex + addtiontionLength, path.Length - (addtiontionLength + startIndex + extentionLength));
    }
}
