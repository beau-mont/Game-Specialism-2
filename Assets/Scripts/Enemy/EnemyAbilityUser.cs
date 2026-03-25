using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyAbilityUser : MonoBehaviour, IAbilityUser
{
    [SerializeField] private LayerMask _TargetLayers;
    public LayerMask TargetLayers { get => _TargetLayers; }
    [SerializeField] private LayerMask _IgnoreLayers;
    public LayerMask IgnoreLayers { get => _IgnoreLayers; }
    [SerializeField] private List<AbilityContainer> availableAbilities;
    [SerializeField] private AbilityContainer _CurrentAbility;
    public AbilityContainer CurrentAbility { get => _CurrentAbility; set => _CurrentAbility = value; }
    public GameObject owner;

     void Start()
    {
        if (owner == null) owner = gameObject;
    }

    public void ActivateAbility()
    {
        if (CurrentAbility.Ability == null && availableAbilities.Count > 0) CycleAbility();
        CurrentAbility.IncludeLayers = TargetLayers;
        CurrentAbility.ExcludeLayers = IgnoreLayers;
        if (CurrentAbility.Ability != null)
        {
            CurrentAbility.Ability.ActivateAbility(owner, new(), CurrentAbility);
        }
        else
        {
            Debug.LogWarning($"No ability selected");
        }
    }

    public void HoldAbility()
    {
        if (CurrentAbility.Ability != null)
        {
            CurrentAbility.Ability.HoldAbility(owner, new(), CurrentAbility);
        }
        else
        {
            Debug.LogWarning($"No ability selected");
        }
    }

    public void DeactivateAbility()
    {
        if (CurrentAbility.Ability != null)
        {
            CurrentAbility.Ability.DeactivateAbility(owner, new(), CurrentAbility);
            if (CurrentAbility.Ability.IsSingleUse)
            {
                RemoveAbility(CurrentAbility);
                CycleAbility();
            }
        }
        else
        {
            Debug.LogWarning($"No ability selected");
            CycleAbility();
        }
    }

    public void AddAbility(AbstractAbility ability)
    {
        if (!availableAbilities.Any(a => a.Ability.AbilityName == ability.AbilityName))
        {
            availableAbilities.Add(new AbilityContainer{Ability = ability, LastFired = 0f, IncludeLayers = TargetLayers, ExcludeLayers = IgnoreLayers});
            // Debug.Log($"Added ability: {ability.AbilityName}");
        }
        else
        {
            Debug.LogWarning($"Ability already added: {ability.AbilityName}");
        }
    }

    public void CycleAbility()
    {
        if (availableAbilities.Count == 0)
        {
            Debug.LogWarning($"No abilities available to cycle to");
            CurrentAbility = null;
            return;
        }
        if (CurrentAbility != null && CurrentAbility.Ability == null) // if just the ability is null then fix
        {
            SetAbility(availableAbilities.FirstOrDefault());
            return;
        }
        int currentIndex = availableAbilities.IndexOf(CurrentAbility);
        int nextIndex = (currentIndex + 1) % availableAbilities.Count;
        SetAbility(availableAbilities[nextIndex]);
    }

    public void RemoveAbility(AbilityContainer ability)
    {
        availableAbilities.Remove(ability);
    }

    public void SetAbility(AbilityContainer ability)
    {
        CurrentAbility = ability;
    }
}
