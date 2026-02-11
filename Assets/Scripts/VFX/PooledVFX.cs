using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a basic implementation of pooling for all other objects.
/// </summary>
[CreateAssetMenu(fileName = "PooledVFX", menuName = "VFX/PooledVFX"), System.Serializable]
public class PooledVFX : ScriptableObject
{
    [SerializeField] private GameObject VFXPrefab;
    private List<GameObject> VFXPool;

    public GameObject GetPooledObject() // try fetch an inactive object from the pool
    {
        VFXPool ??= new List<GameObject>(); // initialize pool if not already
        VFXPool.RemoveAll(proj => proj == null);
        if (VFXPool.Count == 0) // if pool is empty then create a new object
        {
            return CreateNewObject();
        }
        for (int i = 0; i < VFXPool.Count; i++)
        {
            if (!VFXPool[i].activeInHierarchy) return VFXPool[i]; // return first inactive object found
        }
        return CreateNewObject();
    }
    private GameObject CreateNewObject() // create a new object and add it to the pool
    {
        GameObject newObj = Instantiate(VFXPrefab);
        VFXPool.Add(newObj);
        return newObj;
    } 
}
