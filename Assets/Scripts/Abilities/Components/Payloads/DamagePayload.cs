using UnityEngine;

/// <summary>
/// A basic damage payload. deals damage * Multiplier to any IDamageable it hits.
/// </summary>
[CreateAssetMenu(fileName = "DamagePayload", menuName = "Abilities/Payloads/DamagePayload"), System.Serializable, SerializeField]
public class DamagePayload : AbstractPayload
{
    public float baseDamage;
    public override void HitEffect(GameObject projectile, GameObject target, PayloadMultipliers mod)
    {
        float mult = 1 + mod.GlobalMultiplier + mod.DamageMultiplier;
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.ModifyHealth(baseDamage * mult);
            Debug.Log($"dealt {baseDamage * mult} damage");
        }
    }
}