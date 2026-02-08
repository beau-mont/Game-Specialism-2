using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// abstract class for abilities.
/// </summary>
public abstract class AbstractAbility : ScriptableObject
{
    public abstract string AbilityName { get; }
    public abstract float CooldownDuration { get; }
    public abstract bool IsSingleUse { get; }
    public abstract AbilityMultipliers AbilityMultipliers { get; set; }
    protected abstract GameObject ProjectilePrefab { get; } // assign in inspector or via code
    protected abstract List<GameObject> ProjectilePool { get; set; }
    public abstract void ActivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer);
    public abstract void HoldAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer);
    public abstract void DeactivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer);
    
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

/// <summary>
/// Ability multiplier container, stores a list of multipliers/modifiers that is passed to an ability to modify its behaviour.
/// </summary>
/// <param name="GlobalMultiplier">a multiplier that applies equally to every effect.</param>
/// <param name="CooldownMultiplier">a multiplier that only decreases the cooldown of a projectile.</param>
/// <param name="AccuracyMultiplier">a multiplier to increase the accuracy of an ability.</param>
/// <param name="BonusProjectiles">a fixed amount of bonus projectiles to fire. (not yet implemented).</param>
/// <remarks>when implementing these multiply the base value by (1 + GlobalMultiplier + <relevant multipliers> + ect)</remarks>
[System.Serializable]
public class AbilityMultipliers
{
    public PayloadMultipliers PayloadMultipliers;
    public ProjectileMultipliers ProjectileMultipliers;
    public float GlobalMultiplier;
    public float CooldownMultiplier;
    public float AccuracyMultiplier;
    public int BonusProjectiles;
}