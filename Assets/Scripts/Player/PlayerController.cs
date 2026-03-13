using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    //[SerializeField] private float _MaxHealth;
    // when you ask for this objects max health it returns the max health plus the modifier.
    // this means we keep a base max health but we can still modify it easily with rouge-like abilities down the road.
    [SerializeField] private float MaxHealth;
    public float currentHealth;
    [SerializeReference, SerializeField] private List<DamageThreshold> _damageThresholds;
    public List<DamageThreshold> DamageThresholds { get => _damageThresholds; set => _damageThresholds = value; }
    [Header("Settings")]
    public float baseMoveSpeed;
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;
    [Header("VFX")]
    [SerializeField] private List<PooledVFX> deathVFX;
    private Rigidbody2D rb;
    InputAction moveAction;
    InputAction attackAction;
    InputAction switchAction;
    InputAction parryAction;
    InputAction specialAction;
    InputAction pauseAction;
    public UnityAction OnDamage;
    public UnityAction<bool> OnParry; // bool is true if parry was successful, false if it missed.
    public UnityAction OnKillSpecial;
    public UnityAction OnParrySpecial;
    public UnityAction OnQuickStep;
    public UnityAction OnKill;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        switchAction = InputSystem.actions.FindAction("Switch");
        parryAction = InputSystem.actions.FindAction("Parry");
        specialAction = InputSystem.actions.FindAction("Special");
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    void OnEnable()
    {
        if (!playerData)
        {
            Debug.LogWarning($"{gameObject.name} has no player data assigned.");
            return;
        }
        if (playerData)
        {
            playerData.Player = gameObject;
            playerData.PlayerController = this;
        }
        currentHealth = MaxHealth;
        DamageableList.objects.Add(gameObject);
    }

    void OnDisable()
    {
        DamageableList.objects.Remove(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAbilities();
        ProcessMovement();
    }

    void ProcessMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveMultiplier = 1 + playerData.PlayerUpgradeManager.PlayerMultipliers.GlobalMultiplier + playerData.PlayerUpgradeManager.PlayerMultipliers.MoveSpeedMultiplier;
        if (rb)
            rb.linearVelocity = moveInput * (baseMoveSpeed * moveMultiplier);
        else
        {
            Debug.LogWarning($"No rigidbody on player");
        }
        if (transform.position.x > maxX) transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        if (transform.position.x < minX) transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        if (transform.position.y > maxY) transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        if (transform.position.y < minY) transform.position = new Vector3(transform.position.x, minY, transform.position.z);
    }

    void ProcessAbilities()
    { 
        if (!playerData || playerData.PlayerAbilityUser == null) return; // if we cant use abilities then just forget aboot it
        
        if (switchAction.WasPressedThisFrame())// TODO: Replace with a proper controller
        {
            if (attackAction.IsPressed()) playerData.PlayerAbilityUser.DeactivateAbility();
            playerData.PlayerAbilityUser.CycleAbility();
            if (attackAction.IsPressed()) playerData.PlayerAbilityUser.ActivateAbility();
        } 

        // TODO: Replace all of this with a proper controller and move ability usage to the PlayerAbilityUser
        if (attackAction.WasPressedThisFrame())
        {
            playerData.PlayerAbilityUser.ActivateAbility();
        }
        else if (attackAction.IsPressed())
        {
            playerData.PlayerAbilityUser.HoldAbility();
        }
        else if (attackAction.WasReleasedThisFrame())
        {
            playerData.PlayerAbilityUser.DeactivateAbility();
        }
    }

    #region Damageable
    public void ModifyHealth(float value)
    {
        currentHealth -= value;
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
            if (vfx == null) continue;
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
