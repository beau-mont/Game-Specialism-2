using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

/// <summary>
/// Flies in a straight line, it moves upwards at speed * Multiplier
/// </summary>
public class SimpleProjectileBehaviour : AbstractAbilityBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private AbilityBehaviourMultipliers _multipliers;
    public override AbilityBehaviourMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float mult = 1;
        if (Multipliers != null) mult += Multipliers.GlobalMultiplier + Multipliers.SpeedMultiplier;
        transform.position += (mult * speed * transform.up) * Time.deltaTime;
    }
}
