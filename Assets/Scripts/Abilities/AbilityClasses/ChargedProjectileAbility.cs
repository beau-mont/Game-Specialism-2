using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

/// <summary>
/// A container for an ability that charges up before firing
/// </summary>
[CreateAssetMenu(fileName = "ChargedProjectileAbility", menuName = "Abilities/ChargedProjectileAbility")]
public class ChargedProjectileAbility : AbstractAbility 
{
    [Header("Inherited Variables")]
    [SerializeField] private string _abilityName;
    [Description("Name of the ability, only used to give more context for developers")]
    public override string AbilityName => _abilityName; // name of the ability (for devs)
    [SerializeField] private bool _isSingleUse;
    public override bool IsSingleUse => _isSingleUse;
    [SerializeField] private GameObject _projectilePrefab;
    protected override GameObject ProjectilePrefab => _projectilePrefab;
    private List<GameObject> _projectilePool = new List<GameObject>();
    protected override List<GameObject> ProjectilePool { get => _projectilePool; set => _projectilePool = value; }
    [SerializeField] private float _cooldownDuration;
    public override float CooldownDuration => _cooldownDuration;
    [Header("Custom Variables")]
    [SerializeField] private float baseSpeedMult = 0f;
    [SerializeField] private float maxSpeedMult = 1f;
    [SerializeField] private float baseDamageMult = 0f;
    [SerializeField] private float maxDamageMult = 2f;
    [SerializeField] private float maxChargeDuration = 3f;
    [SerializeField] private float maxScaleMult = 1f;
    public PooledVFX[] chargeVFX;
    public PooledVFX[] fireVFX;
    public PooledSFX[] fireSFX;

    public override void ActivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        abilityContainer.LastFired = Time.time;
        foreach(var vfx in chargeVFX) // spawn charge vfx
        {
            GameObject temp = vfx.GetPooledObject();
            temp.transform.SetPositionAndRotation(user.transform.position, user.transform.rotation);
            if (temp.TryGetComponent<VFXComponent>(out var comp))
                comp.multipliers = abilityMultipliers.PayloadMultipliers;
            temp.SetActive(true);
        }
    }

    public override void HoldAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        
    }

    public override void DeactivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        float chargeTime = Mathf.Clamp(Time.time - abilityContainer.LastFired, 0f, maxChargeDuration);
        Debug.Log($"fired projectile with charge time {chargeTime}");
        float chargeMult = Mathf.Clamp01(chargeTime / maxChargeDuration);
        PayloadMultipliers payloadMult = new() // combine provided multipliers with charge bonuses
        { 
            GlobalMultiplier = abilityMultipliers.PayloadMultipliers.GlobalMultiplier, // im dividing some of these by 2 as a balance measure
            DamageMultiplier = abilityMultipliers.PayloadMultipliers.DamageMultiplier + math.lerp(baseDamageMult, maxDamageMult, chargeMult),
            RadiusMultiplier = abilityMultipliers.PayloadMultipliers.RadiusMultiplier + math.lerp(baseDamageMult, maxDamageMult / 2f, chargeMult),
            BurnMultiplier = abilityMultipliers.PayloadMultipliers.BurnMultiplier + math.lerp(baseDamageMult, maxDamageMult / 2f, chargeMult)
        };
        AbilityBehaviourMultipliers projectileMult = new() // combine provided multipliers with charge bonuses
        { 
            GlobalMultiplier = abilityMultipliers.AbilityBehaviourMultipliers.GlobalMultiplier,
            SpeedMultiplier = abilityMultipliers.AbilityBehaviourMultipliers.SpeedMultiplier + math.lerp(baseSpeedMult, maxSpeedMult, chargeMult),
            HomingMultiplier = abilityMultipliers.AbilityBehaviourMultipliers.HomingMultiplier + math.lerp(baseSpeedMult, maxSpeedMult / 2f, chargeMult)
        };

        GameObject projectile = GetPooledObject();
        projectile.transform.SetPositionAndRotation(user.transform.position + (user.transform.up * 0.5f), user.transform.rotation);
        projectile.transform.localScale = Vector3.one * (1f + math.lerp(0f, maxScaleMult, chargeMult));
        if (projectile.TryGetComponent<AbilityDecorator>(out var abilityDecorator))
        {
            abilityDecorator.owner = user;
            abilityDecorator.payloadMultipliers = payloadMult;
            abilityDecorator.abilityBehaviourMultipliers = projectileMult;
        }        
        if (projectile.TryGetComponent<Collider2D>(out var collider2D))
        {
            collider2D.includeLayers = abilityContainer.IncludeLayers;
            collider2D.excludeLayers = abilityContainer.ExcludeLayers;
        }
        
        foreach(var vfx in fireVFX) // spawn fire vfx
        {
            GameObject temp = vfx.GetPooledObject();
            temp.transform.SetPositionAndRotation(user.transform.position, user.transform.rotation);
            if (temp.TryGetComponent<VFXComponent>(out var comp))
                comp.multipliers = abilityMultipliers.PayloadMultipliers;
            temp.SetActive(true);
        }

        if (fireSFX.Count() > 0)
        {
            GameObject tempSFX = fireSFX[UnityEngine.Random.Range(0, fireSFX.Count())].GetPooledObject();
            tempSFX.transform.SetPositionAndRotation(user.transform.position, user.transform.rotation);
            tempSFX.SetActive(true);
        }

        projectile.SetActive(true);
    }
}