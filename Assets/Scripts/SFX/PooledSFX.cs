using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// a static class that can be used to fetch SFX
/// </summary>
[CreateAssetMenu(fileName = "PooledSFX", menuName = "SFX/PooledSFX"), System.Serializable]
public class PooledSFX : ScriptableObject
{
    [SerializeField] private GameObject SFXPrefab;
    private List<GameObject> SFXPool;

    public GameObject GetPooledObject() // try fetch an inactive object from the pool
    {
        SFXPool ??= new List<GameObject>(); // initialize pool if not already
        SFXPool.RemoveAll(proj => proj == null);
        if (SFXPool.Count == 0) // if pool is empty then create a new object
        {
            return CreateNewObject();
        }
        for (int i = 0; i < SFXPool.Count; i++)
        {
            if (!SFXPool[i].activeInHierarchy) return SFXPool[i]; // return first inactive object found
        }
        return CreateNewObject();
    }

    private GameObject CreateNewObject() // create a new object and add it to the pool
    {
        GameObject newObj = Instantiate(SFXPrefab);
        SFXPool.Add(newObj);
        return newObj;
    } 
}