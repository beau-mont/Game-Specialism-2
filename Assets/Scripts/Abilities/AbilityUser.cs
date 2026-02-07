using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class handles the use of abilities
/// </summary>
public class IAbilityUser : MonoBehaviour
{
    public List<IAbility> availableAbilities;
    public IAbility currentAbility;

    void Start()
    {
        List<IAbility> startAbilities = new List<IAbility>(); // swap out abilities given in inspector for new instances
        foreach (var ability in availableAbilities)
        {
            startAbilities.Add(Instantiate(ability));
        }
        availableAbilities = startAbilities;
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
        if (toSet) currentAbility = toSet;
        else Debug.LogError($"Ability {ability} not available for {gameObject.name}");
    }

    public void CycleAbility()
    {
        if (availableAbilities.Count == 0)
        {
            // Debug.Log($"No abilities available to cycle to");
            return;
        }
        
        int currentIndex = availableAbilities.IndexOf(currentAbility != null ? currentAbility : null);
        int nextIndex = (currentIndex + 1) % availableAbilities.Count;
        currentAbility = availableAbilities[nextIndex];
        // Debug.Log($"ability set to {currentAbility.AbilityName}");
    }
}