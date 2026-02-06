using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Singleton for pooling projectile objects
/// </summary>
public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }
    public List<GameObject> pooledObjects = new List<GameObject>();
    public GameObject objectToPool;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject GetPooledObject() // try fetch an inactive object from the pool
    {
        if (pooledObjects.Count == 0) // if pool is empty then create a new object
        {
            return CreateNewObject();
        }
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) return pooledObjects[i]; // return first inactive object found
        }
        return CreateNewObject();
    }

    public GameObject CreateNewObject() // create a new object and add it to the pool
    {
        GameObject newObj = Instantiate(objectToPool);
        pooledObjects.Add(newObj);
        return newObj;
    }
}