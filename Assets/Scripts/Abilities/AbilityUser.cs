using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A monoBehavior that essentially provides an interface to manage the usage of abilities.
/// </summary>
public class PlayerAbilityUser : MonoBehaviour, IAbilityUser
{
    [SerializeField] private List<IAbility> availableAbilities;
    public IAbility CurrentAbility { get; set; }
    [SerializeField] private PlayerData playerData;

    void Start()
    {
        playerData.PlayerAbilityUser = this;
    }
    
    public void AddAbility(IAbility ability)
    {
        if (!availableAbilities.Any(a => a.AbilityName == ability.AbilityName))
        {
            availableAbilities.Add(ability);
            // Debug.Log($"Added ability: {ability.AbilityName}");
        }
        else
        {
            Debug.LogWarning($"Ability already added: {ability.AbilityName}");
        }
    }

    public void RemoveAbility(IAbility ability)
    {
        availableAbilities.Remove(ability);
    }

    public void RemoveAbility(string ability)
    {
        availableAbilities.RemoveAll(a => a.AbilityName == ability);
    }

    public void SetAbility(string ability)
    {
        IAbility toSet = availableAbilities.FirstOrDefault(a => a.AbilityName == ability);
        if (toSet) CurrentAbility = toSet;
        else Debug.LogError($"Ability {ability} not available for {gameObject.name}");
    }

    public void SetAbility(IAbility ability)
    {
        if (ability) CurrentAbility = ability;
        else Debug.LogWarning($"attempt to set invalid ability");
    }

    public void CycleAbility()
    {
        if (availableAbilities.Count == 0)
        {
            // Debug.Log($"No abilities available to cycle to");
            return;
        }
        
        int currentIndex = availableAbilities.IndexOf(CurrentAbility != null ? CurrentAbility : null);
        int nextIndex = (currentIndex + 1) % availableAbilities.Count;
        CurrentAbility = availableAbilities[nextIndex];
        // Debug.Log($"ability set to {currentAbility.AbilityName}");
    }
}

/// <summary>
/// ability user interface
/// </summary>
public interface IAbilityUser
{
    IAbility CurrentAbility { get; set; }
    void AddAbility(IAbility ability);
    void RemoveAbility(IAbility ability);
    void SetAbility(string ability);
    void CycleAbility();
}