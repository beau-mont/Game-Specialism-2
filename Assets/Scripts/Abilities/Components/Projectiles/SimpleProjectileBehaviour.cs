using UnityEngine;

/// <summary>
/// Flies in a straight line, it moves upwards at speed * Multiplier
/// </summary>
public class SimpleProjectileBehaviour : AbstractAbilityBehaviour
{
    public float speed;
    public override void Process(GameObject projectile, Rigidbody2D rb, AbilityBehaviourMultipliers mod)
    {
        float mult = 1 + mod.GlobalMultiplier + mod.SpeedMultiplier;
        if (rb) rb.linearVelocity = mult * speed * projectile.transform.up;
        else Debug.LogWarning($"No Rigidbody2d found on projectile {projectile.name}");
    }
}
