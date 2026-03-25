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

[System.Serializable]
public class PlayerMultipliers
{
    public float GlobalMultiplier;
    public float MoveSpeedMultiplier;
    public float MaxHealthMultiplier;
    public AbilityMultipliers AbilityMultipliers;

    // Overload + operator to add two multipliers
    public static PlayerMultipliers operator+ (PlayerMultipliers a, PlayerMultipliers b) {
        PlayerMultipliers mult = new()
        {
            GlobalMultiplier = a.GlobalMultiplier + b.GlobalMultiplier,
            MoveSpeedMultiplier = a.MoveSpeedMultiplier + b.MoveSpeedMultiplier,
            AbilityMultipliers = a.AbilityMultipliers + b.AbilityMultipliers,
            MaxHealthMultiplier = a.MaxHealthMultiplier + b.MaxHealthMultiplier
        };
        return mult;
    }

    // Overload - operator to subtract two multipliers
    public static PlayerMultipliers operator- (PlayerMultipliers a, PlayerMultipliers b) {
        PlayerMultipliers mult = new()
        {
            GlobalMultiplier = a.GlobalMultiplier - b.GlobalMultiplier,
            MoveSpeedMultiplier = a.MoveSpeedMultiplier - b.MoveSpeedMultiplier,
            AbilityMultipliers = a.AbilityMultipliers - b.AbilityMultipliers,
            MaxHealthMultiplier = a.MaxHealthMultiplier - b.MaxHealthMultiplier
        };
        return mult;
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
    public AbilityBehaviourMultipliers AbilityBehaviourMultipliers;
    public float GlobalMultiplier;
    public float CooldownMultiplier;
    public float AccuracyMultiplier;
    public int BonusProjectiles;

    // Overload + operator to add two multipliers
    public static AbilityMultipliers operator+ (AbilityMultipliers a, AbilityMultipliers b) {
        AbilityMultipliers mult = new()
        {
            PayloadMultipliers = a.PayloadMultipliers + b.PayloadMultipliers,
            AbilityBehaviourMultipliers = a.AbilityBehaviourMultipliers + b.AbilityBehaviourMultipliers,
            GlobalMultiplier = a.GlobalMultiplier + b.GlobalMultiplier,
            CooldownMultiplier = a.CooldownMultiplier + b.CooldownMultiplier,
            AccuracyMultiplier = a.AccuracyMultiplier + b.AccuracyMultiplier,
            BonusProjectiles = a.BonusProjectiles + b.BonusProjectiles
        };
        return mult;
    }

    // Overload - operator to subtract two multipliers
    public static AbilityMultipliers operator- (AbilityMultipliers a, AbilityMultipliers b) {
        AbilityMultipliers mult = new()
        {
            PayloadMultipliers = a.PayloadMultipliers - b.PayloadMultipliers,
            AbilityBehaviourMultipliers = a.AbilityBehaviourMultipliers - b.AbilityBehaviourMultipliers,
            GlobalMultiplier = a.GlobalMultiplier - b.GlobalMultiplier,
            CooldownMultiplier = a.CooldownMultiplier - b.CooldownMultiplier,
            AccuracyMultiplier = a.AccuracyMultiplier - b.AccuracyMultiplier,
            BonusProjectiles = a.BonusProjectiles - b.BonusProjectiles
        };
        return mult;
    }
}

/// <summary>
/// Payload modifier container, this is passed to payloads so they can implement what they need.
/// When adding new multipliers add the new variable here and implement it where needed.
/// </summary>
/// <param name="GlobalMultiplier">a multiplier that applies equally to every effect.</param>
/// <param name="DamageMultiplier">a multiplier that only increases damage of a payload.</param>
/// <param name="RadiusMultiplier">a multiplier to increase the radius of AOE payloads.</param>
/// <param name="BurnMultiplier">a multiplier to increase burn stacks. (not yet implemented).</param>
/// <remarks>when implementing these multiply the base value by (1 + GlobalMultiplier + <relevant multipliers> + ect)</remarks>
[System.Serializable]
public class PayloadMultipliers
{
    public float GlobalMultiplier;
    public float DamageMultiplier;
    public float RadiusMultiplier;
    public float BurnMultiplier;

    // Overload + operator to add two multipliers
    public static PayloadMultipliers operator+ (PayloadMultipliers a, PayloadMultipliers b) {
        PayloadMultipliers mult = new()
        {
            GlobalMultiplier = a.GlobalMultiplier + b.GlobalMultiplier,
            DamageMultiplier = a.DamageMultiplier + b.DamageMultiplier,
            RadiusMultiplier = a.RadiusMultiplier + b.RadiusMultiplier,
            BurnMultiplier = a.BurnMultiplier + b.BurnMultiplier
        };
        return mult;
    }

    // Overload - operator to subtract two multipliers
    public static PayloadMultipliers operator- (PayloadMultipliers a, PayloadMultipliers b) {
        PayloadMultipliers mult = new()
        {
            GlobalMultiplier = a.GlobalMultiplier - b.GlobalMultiplier,
            DamageMultiplier = a.DamageMultiplier - b.DamageMultiplier,
            RadiusMultiplier = a.RadiusMultiplier - b.RadiusMultiplier,
            BurnMultiplier = a.BurnMultiplier - b.BurnMultiplier
        };
        return mult;
    }
}

/// <summary>
/// Projectile modifier container, this is passed to projectiles so they can implement what they need.
/// When adding new multipliers add the new variable here and implement it where needed.
/// </summary>
[System.Serializable]
public class AbilityBehaviourMultipliers
{
    public float GlobalMultiplier; // default all to zero, when implementing these multiply the base value by (1 + GlobalMultiplier + <relevant multipliers> + ect)
    public float SpeedMultiplier;
    public float HomingMultiplier;

    // Overload + operator to add two multipliers
    public static AbilityBehaviourMultipliers operator+ (AbilityBehaviourMultipliers a, AbilityBehaviourMultipliers b) {
        AbilityBehaviourMultipliers mult = new()
        {
            GlobalMultiplier = a.GlobalMultiplier + b.GlobalMultiplier,
            SpeedMultiplier = a.SpeedMultiplier + b.SpeedMultiplier,
            HomingMultiplier = a.HomingMultiplier + b.HomingMultiplier
        };
        return mult;
    }

    // Overload - operator to subtract two multipliers
    public static AbilityBehaviourMultipliers operator- (AbilityBehaviourMultipliers a, AbilityBehaviourMultipliers b) {
        AbilityBehaviourMultipliers mult = new()
        {
            GlobalMultiplier = a.GlobalMultiplier - b.GlobalMultiplier,
            SpeedMultiplier = a.SpeedMultiplier - b.SpeedMultiplier,
            HomingMultiplier = a.HomingMultiplier - b.HomingMultiplier
        };
        return mult;
    }
}
