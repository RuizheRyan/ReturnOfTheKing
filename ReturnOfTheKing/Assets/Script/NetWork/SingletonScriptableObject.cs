using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{

    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length == 0)
                {
                    Debug.LogError("Not instanced");
                    return null;
                }
                if(results.Length > 1)
                {
                    Debug.LogError("More than 1 instances");
                    return null;
                }

                _instance = results[0];
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
