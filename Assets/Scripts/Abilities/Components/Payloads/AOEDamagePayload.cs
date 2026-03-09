using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// THIS IS SO CURSED PLEASE GOD CHANGE THIS LATER
/// </summary>
/// <param name="baseDamage">The amount of damage to deal where time = range to target and value = damage to deal. the last key specifies the max range.</param>
public class AOEDamagePayload : AbstractPayload
{
    public AnimationCurve baseDamage;
    public override void HitEffect(GameObject projectile, GameObject target, PayloadMultipliers mod)
    {
        float radiusMult = 1 + mod.GlobalMultiplier + mod.RadiusMultiplier;
        float damageMult = 1 + mod.GlobalMultiplier + mod.DamageMultiplier;
        DamageableList.objects.RemoveAll(a => a == null); // make sure the list is gucci
        GameObject[] hitObjects = DamageableList.objects.Where(a => (a.transform.position - projectile.transform.position).magnitude < baseDamage.keys.Last().time * radiusMult).ToArray();
        foreach(var hit in hitObjects)
        {
            if (hit == projectile.GetComponent<AbilityDecorator>().owner) // prevents self damage
                return;
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                Debug.Log($"explosion dealt {baseDamage.Evaluate((projectile.transform.position - projectile.transform.position).magnitude) * damageMult} damage");
                damageable.ModifyHealth(baseDamage.Evaluate((projectile.transform.position - projectile.transform.position).magnitude) * damageMult);
            }
        }
    }
}