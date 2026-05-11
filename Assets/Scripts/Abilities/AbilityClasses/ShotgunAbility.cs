using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

/// <summary>
/// A container for a generic full auto projectile ability
/// </summary>
[CreateAssetMenu(fileName = "ShotgunAbility", menuName = "Abilities/ShotgunAbility")]
public class ShotgunAbility : AbstractAbility
{
    [Header("Inherited Variables")]
    [SerializeField] private string _abilityName;
    [Description("Name of the ability, only used to give more context for developers")]
    public override string AbilityName => _abilityName;
    [SerializeField] private bool _isSingleUse;
    public override bool IsSingleUse => _isSingleUse;
    [SerializeField] private float _cooldownDuration;
    public override float CooldownDuration => _cooldownDuration;
    [SerializeField] private GameObject _projectilePrefab;
    protected override GameObject ProjectilePrefab => _projectilePrefab;
    private List<GameObject> _projectilePool = new List<GameObject>();
    protected override List<GameObject> ProjectilePool { get => _projectilePool; set => _projectilePool = value; }
    [Header("Custom Variables")]
    public List<PooledVFX> fireVFX;
    public PooledSFX[] fireSFX;
    public float pelletCount;
    public float spreadPerPellet;

    public override void ActivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        if (Time.time > abilityContainer.LastFired + (CooldownDuration / (1 + abilityMultipliers.GlobalMultiplier + abilityMultipliers.CooldownMultiplier))) TryFire(user, abilityMultipliers, abilityContainer);
    }
    public override void HoldAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        if (Time.time > abilityContainer.LastFired + (CooldownDuration / (1 + abilityMultipliers.GlobalMultiplier + abilityMultipliers.CooldownMultiplier))) TryFire(user, abilityMultipliers, abilityContainer);
    }
    public override void DeactivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {

    }

    private void TryFire(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        for (int i = 0; i < pelletCount + abilityMultipliers.BonusProjectiles; i++)
        {
            GameObject projectile = GetPooledObject();
            float spreadMult = 1 + abilityMultipliers.GlobalMultiplier + abilityMultipliers.AccuracyMultiplier;
            projectile.transform.SetPositionAndRotation(user.transform.position + (user.transform.up * 0.5f), user.transform.rotation);
            projectile.transform.Rotate(user.transform.forward, Random.Range(-spreadPerPellet, spreadPerPellet) * (pelletCount + abilityMultipliers.BonusProjectiles) / spreadMult); // for every bonus projectile, increase spread by 5 degrees
            if (projectile.TryGetComponent<AbilityDecorator>(out var abilityDecorator))
            {
                abilityDecorator.owner = user;
                abilityDecorator.payloadMultipliers = abilityMultipliers.PayloadMultipliers;
                abilityDecorator.abilityBehaviourMultipliers = abilityMultipliers.AbilityBehaviourMultipliers;
            }
            if (projectile.TryGetComponent<Collider2D>(out var collider2D))
            {
                collider2D.includeLayers = abilityContainer.IncludeLayers;
                collider2D.excludeLayers = abilityContainer.ExcludeLayers;
            }
            
            abilityContainer.LastFired = Time.time;
            projectile.SetActive(true);
        }

        foreach (var vfx in fireVFX) // spawn fire vfx
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
    }
}