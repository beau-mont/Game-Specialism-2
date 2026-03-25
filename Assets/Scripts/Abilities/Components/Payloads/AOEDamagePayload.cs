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
    public bool matchY;
    public override void HitEffect(GameObject projectile, GameObject target, PayloadMultipliers mod)
    {
        float radiusMult = 1;
        float damageMult = 1;
        if (mod != null)
        {
            radiusMult += mod.GlobalMultiplier + mod.RadiusMultiplier;
            damageMult += mod.GlobalMultiplier + mod.DamageMultiplier;
        }
        Vector3 adjustedProjectilePos = new(projectile.transform.position.x, target.transform.position.y, projectile.transform.position.z);
        DamageableList.objects.RemoveAll(a => a == null); // make sure the list is gucci
        GameObject[] hitObjects;
        if (matchY)
            hitObjects = DamageableList.objects.Where(a => (a.transform.position - adjustedProjectilePos).magnitude < baseDamage.keys.Last().time * radiusMult).ToArray();
        else
            hitObjects = DamageableList.objects.Where(a => (a.transform.position - projectile.transform.position).magnitude < baseDamage.keys.Last().time * radiusMult).ToArray();
        foreach (var hit in hitObjects)
        {
            if (hit == projectile.GetComponent<AbilityDecorator>().owner) // prevents self damage
                return;
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                if (matchY)
                {
                    float damageToDeal = baseDamage.Evaluate((adjustedProjectilePos - hit.transform.position).magnitude / radiusMult) * damageMult;
                    damageable.ModifyHealth(damageToDeal);
                    Debug.Log($"adjusted pos {adjustedProjectilePos} projectile pos {projectile.transform.position} hit pos {hit.transform.position}");
                    Debug.Log($"explosion dealt {damageToDeal} damage to {hit.name}");
                }
                else
                {
                    Debug.Log($"explosion dealt {baseDamage.Evaluate((projectile.transform.position - projectile.transform.position).magnitude / radiusMult) * damageMult} damage");
                    damageable.ModifyHealth(baseDamage.Evaluate((projectile.transform.position - projectile.transform.position).magnitude / radiusMult) * damageMult);
                }
            }
        }
    }
}