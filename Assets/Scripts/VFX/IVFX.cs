using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Unity.VisualScripting;

/// <summary>
/// VFX Scriptable object, use these because they create their own object pools.
/// because they create their own object pools, you should not create instances of these.
/// </summary>
public abstract class IPooledVFX : ScriptableObject
{
    public abstract GameObject VFXPrefab { get; }
    protected abstract List<GameObject> VFXPool { get; set; }
    public abstract GameObject GetPooledObject();
    protected abstract GameObject CreateNewObject();
}

/// <summary>
/// Defines a basic implimentation of pooling for all other objects.
/// Other VFX should inherit from <VFX> and not <IVFX> but other objects should get VFX from <IVFX>
/// </summary>
public class PooledVFX : IPooledVFX
{
    public override GameObject VFXPrefab => VFXPrefab;
    protected override List<GameObject> VFXPool { get => VFXPool; set => VFXPool = value; }

    public override GameObject GetPooledObject() // try fetch an inactive object from the pool
    {
        if (VFXPool == null) VFXPool = new List<GameObject>(); // initialize pool if not already
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
    protected override GameObject CreateNewObject() // create a new object and add it to the pool
    {
        GameObject newObj = Instantiate(VFXPrefab);
        VFXPool.Add(newObj);
        return newObj;
    } 
}

[System.Serializable]
public abstract class VFXStrategy : ScriptableObject
{
    /// <summary>
    /// Set the starting conditions for the VFX
    /// </summary>
    /// <param name="args">VFX data, update the class when you need to move more information here</param>
    public abstract void Begin(VFX_Data args);
    /// <summary>
    /// Process the VFX
    /// </summary>
    /// <param name="args">VFX data, update the class when you need to move more information here</param>
    public abstract void Process(VFX_Data args);
    /// <summary>
    /// Reset the VFX object to starting condition
    /// </summary>
    /// <param name="args">VFX data, update the class when you need to move more information here</param>
    public abstract void End(VFX_Data args);
}

