using UnityEngine;

/// <summary>
/// An abstract class defining a method for processing an abilities behaviour.
/// </summary>
public abstract class AbstractAbilityBehaviour : MonoBehaviour
{
    public abstract AbilityBehaviourMultipliers Multipliers { get; set; }
}
