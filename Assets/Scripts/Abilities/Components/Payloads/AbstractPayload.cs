using System.Collections;
using UnityEngine;

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