using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// abstract class for ability containers
/// </summary>
public abstract class IAbility : ScriptableObject
{
    public abstract string AbilityName { get; }
    public abstract float CooldownDuration { get; }
    public abstract bool IsSingleUse { get; }
    protected abstract GameObject ProjectilePrefab { get; } // assign in inspector or via code
    protected abstract List<GameObject> ProjectilePool { get; set; }
    public abstract void ActivateAbility(IAbilityUser user);
    public abstract void DeactivateAbility(IAbilityUser user);
    public abstract void HoldAbility(IAbilityUser user);
    public abstract void Reset();
    
    protected GameObject GetPooledObject() // try fetch an inactive object from the pool
    {
        if (ProjectilePool == null) ProjectilePool = new List<GameObject>(); // initialize pool if not already
        ProjectilePool.RemoveAll(proj => proj == null);
        if (ProjectilePool.Count == 0) // if pool is empty then create a new object
        {
            return CreateNewObject();
        }
        for (int i = 0; i < ProjectilePool.Count; i++)
        {
            if (!ProjectilePool[i].activeInHierarchy) return ProjectilePool[i]; // return first inactive object found
        }
        return CreateNewObject();
    }

    public GameObject CreateNewObject() // create a new object and add it to the pool
    {
        GameObject newObj = Instantiate(ProjectilePrefab);
        ProjectilePool.Add(newObj);
        return newObj;
    }
}