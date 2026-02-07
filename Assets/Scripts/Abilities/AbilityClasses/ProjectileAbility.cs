using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

[CreateAssetMenu(fileName = "ProjectileAbility", menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : IAbility
{
    [Header("Inherited Variables")]
    [SerializeField] private string _abilityName;
    [Description("Name of the ability, only used to give more context for developers")]
    public override string AbilityName => _abilityName;
    [SerializeField] private bool _isSingleUse;
    public override bool IsSingleUse => _isSingleUse;
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

    public override void Reset()
    {
        readyAt = 0f;
    }

    public override void ActivateAbility(IAbilityUser user)
    {
        if (Time.time > readyAt) TryFire(user);
    }

    public override void DeactivateAbility(IAbilityUser user)
    {
        if (IsSingleUse) user.RemoveAbility(this);
    }

    public override void HoldAbility(IAbilityUser user)
    {
        if (Time.time > readyAt) TryFire(user);
    }

    private void TryFire(IAbilityUser user)
    {
        projectile = null;
        projectile = GetPooledObject();
        if (projectile.TryGetComponent<ProjectileComponent>(out var projectileComponent))
        {
            projectileComponent.projectileName = AbilityName + " Projectile"; // Configure projectile
            projectileComponent.speed = projectileSpeed;
            projectileComponent.damage = projectileDamage;
            projectileComponent.owner = user.gameObject;
            projectileComponent.hitLayers = hitLayers;
            projectileComponent.excludeLayers = excludeLayers;
            projectile.transform.position = user.transform.position + user.transform.up * 0.5f; // spawn in front of user
            projectile.transform.rotation = user.transform.rotation;
        }
        else
        {
            Debug.LogError("Pooled object does not have a ProjectileComponent.");
            return;
        }
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