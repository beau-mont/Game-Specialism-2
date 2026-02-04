using TMPro;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Interface for Abilities
/// </summary>
public abstract class IAbility : ScriptableObject
{
    public abstract string AbilityName { get; }
    public abstract void ActivateAbility(GameObject user);
    public abstract void DeactivateAbility(GameObject user);
    public abstract void HoldAbility(GameObject user);
}

[CreateAssetMenu(fileName = "ProjectileAbility", menuName = "Abilities/ProjectileAbility")]
public class ProjectileAbility : IAbility
{
    public override string AbilityName => "Projectile Ability";
    public GameObject effectPrefab;
    public Sprite abilityIcon;

    public override void ActivateAbility(GameObject user)
    {
        GameObject effect = GameObject.Instantiate(effectPrefab, user.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().sprite = abilityIcon;
        Debug.Log($"{AbilityName} activated on {user.name}");
    }
    public override void DeactivateAbility(GameObject user)
    {
        Debug.Log($"{AbilityName} deactivated on {user.name}");
    }
    public override void HoldAbility(GameObject user)
    {
        Debug.Log($"{AbilityName} holding on {user.name}");
    }
}