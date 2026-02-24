using UnityEngine;

/// <summary>
/// A basic damage payload. deals damage * Multiplier to any IDamageable it hits.
/// </summary>
public class DamagePayload : AbstractPayload
{
    public float damage;
    public override void HitEffect(GameObject projectile, GameObject target, PayloadMultipliers mod)
    {
        float mult = 1 + mod.GlobalMultiplier + mod.DamageMultiplier;
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.ModifyHealth(damage * mult);
            Debug.Log($"dealt {damage * mult} damage");
        }
    }
}