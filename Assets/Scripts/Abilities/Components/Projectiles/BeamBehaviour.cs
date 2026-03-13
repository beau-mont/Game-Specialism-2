using UnityEngine;

[RequireComponent(typeof(Collider2D))]

/// <summary>
/// Flies in a straight line, it moves upwards at speed * Multiplier
/// </summary>
public class BeamBehaviour : AbstractAbilityBehaviour
{
    public float hitboxLifetime;
    private Collider2D ourCollider;
    private float startTime;
    private AbilityBehaviourMultipliers _multipliers;
    public override AbilityBehaviourMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }

    void OnEnable()
    {
        ourCollider = GetComponent<Collider2D>();
        startTime = Time.time;
        ourCollider.enabled = true;
    }

    void Update()
    {
        if (Time.time > startTime + hitboxLifetime && ourCollider.enabled) ourCollider.enabled = false;
    }
}
