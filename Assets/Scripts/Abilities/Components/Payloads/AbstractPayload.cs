using System.Collections;
using UnityEngine;

/// <summary>
/// Payload modifier container, this is passed to payloads so they can implement what they need.
/// When adding new multipliers add the new variable here and implement it where needed.
/// </summary>
/// <param name="GlobalMultiplier">a multiplier that applies equally to every effect.</param>
/// <param name="DamageMultiplier">a multiplier that only increases damage of a payload.</param>
/// <param name="RadiusMultiplier">a multiplier to increase the radius of AOE payloads.</param>
/// <param name="BurnMultiplier">a multiplier to increase burn stacks. (not yet implemented).</param>
/// <remarks>when implementing these multiply the base value by (1 + GlobalMultiplier + <relevant multipliers> + ect)</remarks>
[System.Serializable]
public class PayloadMultipliers
{
    public float GlobalMultiplier;
    public float DamageMultiplier;
    public float RadiusMultiplier;
    public float BurnMultiplier;
}

/// <summary>
/// An abstract class defining a method for all payloads to use
/// </summary>
public abstract class AbstractPayload : ScriptableObject
{
    /// <summary>
    /// Applies its unique payload effect to the target, making use of PayloadMultipliers.
    /// </summary>
    /// <param name="ability">the GameObject of the ability.</param>
    /// <param name="target">the GameObject that was hit.</param>
    /// <param name="mod">the PayloadMultipliers to modify the behaviour of the payload.</param>
    public abstract void HitEffect(GameObject ability, GameObject target, PayloadMultipliers mod);
}