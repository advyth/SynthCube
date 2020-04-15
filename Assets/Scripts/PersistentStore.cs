using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentStore : MonoBehaviour
{
    // Start is called before the first frame update
    private static Dictionary<string, string> Store = new Dictionary<string, string>();
    void Start()
    {
        
    }
    public void AddValue(string key, string val)
    {
        if (Store.ContainsKey(key))
        {
            if (Store[key] != val)
            {
                Store.Remove(key);
                Store.Add(key, val);
            }
        }
        else
        {
            Store.Add(key, val);
        }
       
    }
    public string GetValue(string key)
    {
        if (Store.ContainsKey(key))
        {
            return Store[key];
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
