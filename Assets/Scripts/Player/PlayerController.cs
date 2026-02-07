using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player controller, this provides behaviors for the player.
/// currently depends on AbilityUser though it throws no errors, please remember to fix this dependency with an interface.
/// </summary>
public class PlayerController : MonoBehaviour, IDamageable, IDamageThreshold
{
    [Header("Config SO")]
    [SerializeField] private PlayerData playerData; // global gameobject for access to player data
    [SerializeField] private IAbilityUser abilityUser; // script that runs abilities, will throw exceptions if none is assigned
    [Header("Health")]
    public float maxHealthMod;
    [SerializeField] private float _MaxHealth;
    // when you ask for this objects max health it returns the max health plus the modifier.
    // this means we keep a base max health but we can still modify it easily with rouge-like abilities down the road.
    public float MaxHealth { get => _MaxHealth + maxHealthMod; } 
    public float currentHealth;
    [SerializeReference, SerializeField] private List<DamageThreshold> _damageThresholds;
    public List<DamageThreshold> DamageThresholds { get => _damageThresholds; set => _damageThresholds = value; }
    [Header("VFX")]
    [SerializeField] private List<IPooledVFX> deathVFX;

    
    void OnEnable()
    {
        if (!abilityUser)
        {
            abilityUser = GetComponent<IAbilityUser>();
            Debug.LogWarning($"{gameObject.name} has no abilityUser assigned in inspector; please assign one in the inspector");
        }
        if (!abilityUser) Debug.LogWarning($"No abilityUser attached to player object; please add one to the player object");
        else playerData.PlayerAbilityUser = abilityUser;
        if (!playerData)
        {
            Debug.LogWarning($"{gameObject.name} has no player data assigned.");
            return;
        }
        playerData.PlayerController = this;
        playerData.Start();
        playerData.Player = gameObject;
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        try // TODO: switch away from try-catch
        {
            if (!Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Mouse1)) abilityUser.CycleAbility(); // TODO: Replace with a proper controller
            if (!abilityUser.currentAbility) abilityUser.CycleAbility();
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{gameObject.name} cant cycle ability; {ex}");
        }

        try // TODO: Replace all of this with a proper controller and switch away from try-catch
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                abilityUser.currentAbility.ActivateAbility(abilityUser);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                abilityUser.currentAbility.HoldAbility(abilityUser);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                abilityUser.currentAbility.DeactivateAbility(abilityUser);
                if (abilityUser.currentAbility.IsSingleUse)
                {
                    abilityUser.availableAbilities.RemoveAll(a => a == abilityUser.currentAbility);
                    abilityUser.CycleAbility();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{gameObject.name} caught exception processing fire input; {ex}");
        }

        if (currentHealth < 0f)
        {
            
        }
    }

    #region Damageable
    public void ModifyHealth(float health)
    {
        currentHealth -= health;
        if (currentHealth <= 0f) Kill();
        if (currentHealth > MaxHealth) currentHealth = MaxHealth;
        CheckDamageThresholds();
    }

    public void Kill()
    {
        Debug.Log($"{gameObject.name} has died");
        foreach (var vfx in deathVFX)
        {
            GameObject temp = vfx.GetPooledObject();
            temp.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
        gameObject.SetActive(false);
    }

    [ContextMenu("CheckThresholds")]
    public void CheckDamageThresholds() // only run when HP changes
    {
        float hp = currentHealth / MaxHealth;
        foreach (var threshold in DamageThresholds)
        {
            //Debug.Log($"{threshold.name} config: low threshold = {threshold.LowThreshold}. high threshold = {threshold.HighThreshold}");
            if (threshold.LowThreshold < hp && threshold.HighThreshold > hp)
            {
                if (threshold.Active) threshold.Action();
                else threshold.Start();
                threshold.Active = true;
            }
            else if (threshold.Active) 
            { 
                threshold.End();
                threshold.Active = false;
            } 
        }
    }
    #endregion
}
