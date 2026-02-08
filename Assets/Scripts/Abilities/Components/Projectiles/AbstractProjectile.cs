using UnityEngine;

/// <summary>
/// Projectile modifier container, this is passed to projectiles so they can implement what they need.
/// When adding new multipliers add the new variable here and implement it where needed.
/// </summary>
[System.Serializable]
public class ProjectileMultipliers
{
    public float GlobalMultiplier; // default all to zero, when implementing these multiply the base value by (1 + GlobalMultiplier + <relevant multipliers> + ect)
    public float SpeedMultiplier;
    public float HomingMultiplier;
}

/// <summary>
/// An abstract class defining a method for processing an abilities behaviour.
/// </summary>
public abstract class AbstractAbilityBehaviour : ScriptableObject
{
    public abstract void Process(GameObject ability, Rigidbody2D rb, ProjectileMultipliers mod);
}
