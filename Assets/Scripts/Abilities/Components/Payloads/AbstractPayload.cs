using System.Collections;
using UnityEngine;

/// <summary>
/// An abstract class defining a method for all payloads to use
/// </summary>
[System.Serializable]
public abstract class AbstractPayload : MonoBehaviour
{
    /// <summary>
    /// Applies its unique payload effect to the target, making use of PayloadMultipliers.
    /// </summary>
    /// <param name="ability">the GameObject of the ability.</param>
    /// <param name="target">the GameObject that was hit.</param>
    /// <param name="mod">the PayloadMultipliers to modify the behaviour of the payload.</param>
    /// <param name="baseValue">the base value for the effect to use</param>
    public abstract void HitEffect(GameObject ability, GameObject target, PayloadMultipliers mod);
}