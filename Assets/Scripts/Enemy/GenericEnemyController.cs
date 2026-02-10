using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A generic enemy controller to implement IDamageable for prototyping.
/// this is likely to be reworked or replaced at some point with a sort of decorator.
/// </summary>
public class GenericEnemyController : MonoBehaviour, IDamageable
{

    [Header("Config SO")]
    [SerializeField] private PlayerData playerData; // global gameobject for access to player data

    [Header("Health")] // Health related variables
    public float maxHealthMod;
    // when you ask for this objects max health it returns the max health plus the modifier.
    // this means we keep a base max health but we can still modify it easily with rouge-like abilities down the road.
    [SerializeField] private float MaxHealth; 
    public float currentHealth;
    [SerializeReference, SerializeField] private List<DamageThreshold> _damageThresholds;
    public List<DamageThreshold> DamageThresholds { get => _damageThresholds; set => _damageThresholds = value; }
    [Header("VFX")]
    [SerializeField] private List<IPooledVFX> deathVFX;

    void Start()
    {
        currentHealth = MaxHealth + maxHealthMod;
    }

    void Update()
    {
        
    }

    #region Damageable
    public void ModifyHealth(float health)
    {
        currentHealth -= health;
        if (currentHealth <= 0f) Kill();
        if (currentHealth > (MaxHealth + maxHealthMod)) currentHealth = MaxHealth + maxHealthMod;
        CheckDamageThresholds();
    }

    public void ModifyMaxHealth(float value)
    {
        maxHealthMod += value;
    }

    public void ResetMaxHealth()
    {
        maxHealthMod = 0;
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
