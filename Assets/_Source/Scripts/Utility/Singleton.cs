/**
 * Singleton.cs
 * Created by: Pedro Borges
 * Created on: 13/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<T>();
                if (!instance)
                    instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
            }
            return instance;
        }
    }

    static T instance;

    protected virtual void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarningFormat("Cleaning duplicated singleton object of type {0}", typeof(T).Name);
            return;
        }
        instance = GetComponent<T>();
        if (transform.parent != null)
            transform.SetParent(null, true);
        DontDestroyOnLoad(gameObject);
        
    }
}