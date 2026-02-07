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
        if (!playerData)
        {
            Debug.LogWarning($"{gameObject.name} has no player data assigned.");
            return;
        }
        playerData.PlayerController = this;
        playerData.Player = gameObject;
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAbilities();
    }

    void ProcessAbilities()
    {
        if (!playerData || playerData.PlayerAbilityUser == null) return; // if we cant use abilities then just forget aboot it
        
        if (!Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Mouse1)) playerData.PlayerAbilityUser.CycleAbility(); // TODO: Replace with a proper controller
        if (!playerData.PlayerAbilityUser.CurrentAbility) playerData.PlayerAbilityUser.CycleAbility();

        // TODO: Replace all of this with a proper controller
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerData.PlayerAbilityUser.CurrentAbility.ActivateAbility(playerData.Player);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            playerData.PlayerAbilityUser.CurrentAbility.HoldAbility(playerData.Player);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerData.PlayerAbilityUser.CurrentAbility.DeactivateAbility(playerData.Player);
            if (playerData.PlayerAbilityUser.CurrentAbility.IsSingleUse)
            {
                playerData.PlayerAbilityUser.RemoveAbility(playerData.PlayerAbilityUser.CurrentAbility);
                playerData.PlayerAbilityUser.CycleAbility();
            }
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
