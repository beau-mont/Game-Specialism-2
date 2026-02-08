using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// A container for a generic full auto projectile ability
/// </summary>
[CreateAssetMenu(fileName = "ProjectileAbility", menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : AbstractAbility 
{
    [Header("Inherited Variables")]
    [SerializeField] private string _abilityName;
    [Description("Name of the ability, only used to give more context for developers")]
    public override string AbilityName => _abilityName;
    [SerializeField] private bool _isSingleUse;
    public override bool IsSingleUse => _isSingleUse;
    public override AbilityMultipliers AbilityMultipliers { get; set; }
    [SerializeField] private float cooldownDuration;
    public override float CooldownDuration => cooldownDuration;
    [SerializeField] private GameObject _projectilePrefab;
    protected override GameObject ProjectilePrefab => _projectilePrefab;
    private List<GameObject> _projectilePool = new List<GameObject>();
    protected override List<GameObject> ProjectilePool { get => _projectilePool; set => _projectilePool = value; }
    [Header("Custom Variables")]
    public float projectileSpeed = 10f;
    public float projectileDamage = 1f;
    public LayerMask hitLayers;
    public LayerMask excludeLayers;
    public List<IPooledVFX> fireVFX;
    private GameObject projectile;
    private float readyAt = 0f;

    public override void ActivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        if (Time.time > readyAt) TryFire(user, abilityMultipliers, abilityContainer);
    }
    public override void HoldAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        if (Time.time > readyAt) TryFire(user, abilityMultipliers, abilityContainer);
    }
    public override void DeactivateAbility(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        // if (IsSingleUse) user.RemoveAbility(this); // This is handled by the ability user now.
    }

    private void TryFire(GameObject user, AbilityMultipliers abilityMultipliers, AbilityContainer abilityContainer)
    {
        ProjectilePool ??= new List<GameObject>(); // initialize pool if not already
        projectile = null;
        projectile = GetPooledObject();
        // if (projectile.TryGetComponent<ProjectileComponent>(out var projectileComponent))
        // {
        //     projectileComponent.projectileName = AbilityName + " Projectile"; // Configure projectile
        //     projectileComponent.speed = projectileSpeed;
        //     projectileComponent.Damage = projectileDamage;
        //     projectileComponent.owner = user;
        //     projectileComponent.hitLayers = hitLayers;
        //     projectileComponent.excludeLayers = excludeLayers;
        //     projectile.transform.SetPositionAndRotation(user.transform.position + user.transform.up * 0.5f, user.transform.rotation);
        // }
        // else
        // {
        //     Debug.LogError("Pooled object does not have a ProjectileComponent.");
        //     return;
        // }
        // Debug.Log($"{AbilityName} activated on {user.name}");
        projectile.SetActive(true);
        if (fireVFX.Count > 0)
        {
            foreach (var vfx in fireVFX)
            {
                GameObject tempVFX = vfx.GetPooledObject();
                tempVFX.transform.SetPositionAndRotation(user.transform.position, user.transform.rotation);
            }
        }
        readyAt = Time.time + CooldownDuration;
    }
}