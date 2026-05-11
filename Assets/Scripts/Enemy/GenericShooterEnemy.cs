using System;
using UnityEngine;

/// <summary>
/// fuck doing generic shit for this highkey
/// </summary>
public class GenericShooterEnemy : MonoBehaviour
{
    private IAbilityUser abilityUser;
    [SerializeField] private float minAttackInterval;
    [SerializeField] private float maxAttackInterval;
    [SerializeField] private float holdAttackTime;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float lerpSpeed = 5f;
    public bool facePlayer;
    private float nextAttack;
    private AssaulterBoidState state;

    void Start()
    {
        abilityUser = GetComponent<IAbilityUser>();
        abilityUser ??= GetComponentInParent<IAbilityUser>();
        if (abilityUser == null)
        {
            Debug.LogError($"No IAbilityUser component found on {name} or its parents");
        }
        nextAttack = Time.time + UnityEngine.Random.Range(minAttackInterval, maxAttackInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (facePlayer)
        {
            if (playerData != null)
            {
                Vector3 targetVec = playerData.Player.transform.position - transform.position;
                transform.up = Vector3.Lerp(transform.up, targetVec, Time.deltaTime * lerpSpeed);
            }
            else
            {
                Debug.LogWarning($"Player data not assigned to {name} when expected");
            }
        }
        UpdateState();
        Attack();
    }

    void UpdateState()
    {
        if (state == AssaulterBoidState.Idle && Time.time >= nextAttack)
        {
            state = AssaulterBoidState.StartingAttack;
            return;
        }
        if (state == AssaulterBoidState.StartingAttack)
        {
            state = AssaulterBoidState.HoldingAttack;
            return;
        }
        if (state == AssaulterBoidState.HoldingAttack && Time.time >= nextAttack + holdAttackTime)
        {
            state = AssaulterBoidState.EndingAttack;
            return;
        }
    }

    void Attack()
    {
        switch (state)
        {
            case AssaulterBoidState.Idle:
                break;
            case AssaulterBoidState.StartingAttack:
                abilityUser.ActivateAbility();
                break;
            case AssaulterBoidState.HoldingAttack:
                abilityUser.HoldAbility();
                break;
            case AssaulterBoidState.EndingAttack:
                abilityUser.DeactivateAbility();
                nextAttack = Time.time + UnityEngine.Random.Range(minAttackInterval, maxAttackInterval);
                state = AssaulterBoidState.Idle;
                break;
        }
    }

    enum AssaulterBoidState
    {
        Idle,
        StartingAttack,
        HoldingAttack,
        EndingAttack
    }
}

