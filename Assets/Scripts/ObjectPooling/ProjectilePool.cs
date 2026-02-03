using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Simple object pooling system for projectiles
/// MAYBE REPLACE THIS WITH A SIMPLE FACTORY PATTERN LATER
/// </summary>

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;

    void Awake()
    {
        instance = this;
    }

    public GameObject GetPooledObjectOfType(ProjectileType type) // try fetch an inactive object from the pool
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].GetComponent<Projectile>().ProjectileType == type) return pooledObjects[i];
        }
        try { return CreateNewObject(type); } // try create a new object if none are available
        catch (System.Exception e) // catch any errors during instantiation
        {
            Debug.LogError("Failed to create new pooled object: " + e.Message);
            return null;
        }
    }

    public GameObject CreateNewObject(ProjectileType type) // create a new object and add it to the pool
    {
        GameObject newObj = Instantiate(objectToPool);
        newObj.GetComponent<Projectile>().ProjectileType = type; // change this later when implimentation comes around
        pooledObjects.Add(newObj);
        return newObj;
    }
}

public enum ProjectileType
{
    Basic,
    Fast
}

public class Projectile : MonoBehaviour
{
    public ProjectileType ProjectileType;
}