using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PooledBoids", menuName = "Enemy/PooledBoids")]
public class PooledBoids : ScriptableObject
{
    [SerializeField] private GameObject BoidPrefab;
    private List<GameObject> BoidPool;

    public GameObject GetPooledObject() // try fetch an inactive object from the pool
    {
        BoidPool ??= new List<GameObject>(); // initialize pool if not already
        BoidPool.RemoveAll(proj => proj == null);
        if (BoidPool.Count == 0) // if pool is empty then create a new object
        {
            return CreateNewObject();
        }
        for (int i = 0; i < BoidPool.Count; i++)
        {
            if (!BoidPool[i].activeInHierarchy) return BoidPool[i]; // return first inactive object found
        }
        return CreateNewObject();
    }

    private GameObject CreateNewObject() // create a new object and add it to the pool
    {
        GameObject newObj = Instantiate(BoidPrefab);
        BoidPool.Add(newObj);
        return newObj;
    } 
}
