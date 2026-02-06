using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

[CreateAssetMenu(fileName = "ChargedProjectileAbility", menuName = "Abilities/ChargedProjectileAbility")]
public class ChargedProjectileAbility : IAbility
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
    private float cooldownDuration; // theres no implimentation of a cooldown for this ability so we leave it private
    public override float CooldownDuration => cooldownDuration;
    [Header("Custom Variables")]
    public float baseSpeed = 10f;
    public float baseDamage = 1f;
    public float maxSpeed = 30f;
    public float maxDamage = 10f;
    public float maxChargeDuration = 3f;
    public float maxScale = 2f;
    public LayerMask hitLayers;
    public LayerMask excludeLayers;
    private GameObject projectile;
    private float chargeTime = 0f;
    private Vector3 projectileInitialScale;
    private float multiplier = 0f;

    public override void Reset()
    {
        
    }

    public override void ActivateAbility(IAbilityUser user)
    {
        if (ProjectilePool == null) ProjectilePool = new List<GameObject>(); // initialize pool if not already
        Debug.Log($"{AbilityName} starting to charge on {user.name}");
        projectile = null;
        chargeTime = 0f;
        multiplier = 0f;
        projectile = GetPooledObject();
        if (projectile.TryGetComponent<ProjectileComponent>(out var projectileComponent))
        {
            projectileComponent.owner = user.gameObject;
            projectileComponent.hitLayers = hitLayers;
            projectileComponent.excludeLayers = excludeLayers;
            projectileComponent.projectileName = AbilityName + " Projectile"; // Configure projectile
        }
        projectileInitialScale = projectile.transform.localScale;
        projectile.SetActive(true);
    }
    public override void DeactivateAbility(IAbilityUser user)
    {
        if (projectile.TryGetComponent<ProjectileComponent>(out var projectileComponent))
        {
            projectile.transform.position = user.transform.position + user.transform.up * 0.5f;
            projectile.transform.rotation = user.transform.rotation;
            projectileComponent.speed = math.lerp(baseSpeed, maxSpeed, multiplier);
            projectileComponent.damage = math.lerp(baseDamage, maxDamage, multiplier);
            projectile.transform.localScale = projectileInitialScale * math.lerp(1f, maxScale, multiplier);
            Debug.Log($"{AbilityName} released on {user.name} with charge time of {chargeTime} seconds, speed: {projectileComponent.speed}, damage: {projectileComponent.damage}");
        }
        else
        {
            Debug.LogError("Pooled object does not have a ProjectileComponent.");
            return;
        }
        if (IsSingleUse) user.RemoveAbility(this);
    }

    public override void HoldAbility(IAbilityUser user)
    {
        chargeTime += Time.deltaTime;
        multiplier = Mathf.Clamp01(chargeTime / maxChargeDuration);
        projectile.transform.localScale = projectileInitialScale * math.lerp(1f, maxScale, multiplier);
        projectile.transform.position = user.transform.position + user.transform.up * 0.5f;
        projectile.transform.rotation = user.transform.rotation;
    }
}