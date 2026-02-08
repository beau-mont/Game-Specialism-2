using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A monoBehavior that manages the players abilities
/// </summary>
public class PlayerAbilityUser : MonoBehaviour, IAbilityUser
{
    [SerializeField] private List<AbilityContainer> availableAbilities;
    [SerializeField] private AbilityContainer _CurrentAbility;
    public AbilityContainer CurrentAbility { get => _CurrentAbility; set => _CurrentAbility = value; }
    [SerializeField] private PlayerData playerData;
    [SerializeField] private LayerMask _TargetLayers;
    public LayerMask TargetLayers { get => _TargetLayers; }
    [SerializeField] private LayerMask _IgnoreLayers;
    public LayerMask IgnoreLayers { get => _IgnoreLayers; }

    void Start()
    {
        playerData.PlayerAbilityUser = this; // add yourself to the player data
    }

    public void ActivateAbility()
    {
        CurrentAbility.IncludeLayers = TargetLayers;
        CurrentAbility.ExcludeLayers = IgnoreLayers;
        if (CurrentAbility.Ability)
        {
            CurrentAbility.Ability.ActivateAbility(playerData.Player, playerData.AbilityMultipliers, CurrentAbility);
        }
        else
        {
            Debug.LogWarning($"No ability selected");
        }
    }

    public void HoldAbility()
    {
        if (CurrentAbility.Ability)
        {
            CurrentAbility.Ability.HoldAbility(playerData.Player, playerData.AbilityMultipliers, CurrentAbility);
        }
        else
        {
            Debug.LogWarning($"No ability selected");
        }
    }

    public void DeactivateAbility()
    {
        if (CurrentAbility.Ability)
        {
            CurrentAbility.Ability.DeactivateAbility(playerData.Player, playerData.AbilityMultipliers, CurrentAbility);
        }
        else
        {
            Debug.LogWarning($"No ability selected");
        }
        if (CurrentAbility.Ability.IsSingleUse)
        {
            RemoveAbility(CurrentAbility);
            CycleAbility();
        }
    }
    
    #region helper methods
    public void AddAbility(AbstractAbility ability)
    {
        if (!availableAbilities.Any(a => a.Ability.AbilityName == ability.AbilityName))
        {
            availableAbilities.Add(new AbilityContainer{Ability = ability, LastFired = 0f});
            // Debug.Log($"Added ability: {ability.AbilityName}");
        }
        else
        {
            Debug.LogWarning($"Ability already added: {ability.AbilityName}");
        }
    }

    public void RemoveAbility(AbstractAbility ability)
    {
        availableAbilities.RemoveAll(a => a.Ability == ability);
    }

    public void RemoveAbility(AbilityContainer ability)
    {
        availableAbilities.Remove(ability);
    }

    public void RemoveAbility(string ability)
    {
        RemoveAbility(availableAbilities.FirstOrDefault(a => a.Ability.AbilityName == ability));
    }

    public void SetAbility(string ability)
    {
        AbilityContainer toSet = availableAbilities.FirstOrDefault(a => a.Ability.AbilityName == ability);
        if (toSet != null) SetAbility(toSet);
        else Debug.LogError($"Ability {ability} not available for {gameObject.name}");
    }

    public void SetAbility(AbstractAbility ability)
    {
        if (availableAbilities.Any(a => a.Ability == ability)) CurrentAbility = availableAbilities.FirstOrDefault(a => a.Ability == ability);
        else Debug.LogWarning($"attempt to set invalid ability");
    }

    public void SetAbility(AbilityContainer ability)
    {
        CurrentAbility = ability;
    }

    public void CycleAbility()
    {
        if (availableAbilities.Count == 0)
        {
            // Debug.Log($"No abilities available to cycle to");
            return;
        }
        
        int currentIndex = availableAbilities.IndexOf(CurrentAbility);
        int nextIndex = (currentIndex + 1) % availableAbilities.Count;
        SetAbility(availableAbilities[nextIndex]);
        // Debug.Log($"ability set to {currentAbility.AbilityName}");
    }
    #endregion
}

/// <summary>
/// ability user interface
/// </summary>
public interface IAbilityUser
{
    LayerMask TargetLayers { get; }
    LayerMask IgnoreLayers { get; }
    AbilityContainer CurrentAbility { get; set; }
    void AddAbility(AbstractAbility ability);
    void RemoveAbility(AbstractAbility ability);
    void SetAbility(string ability);
    void CycleAbility();
    void ActivateAbility();
    void HoldAbility();
    void DeactivateAbility();
}

[System.Serializable]
public class AbilityContainer
{
    public AbstractAbility Ability;
    public float LastFired;
    public LayerMask IncludeLayers;
    public LayerMask ExcludeLayers;
}