using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// An interface for damageable entities.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Removes value from currentHealth.
    /// </summary>
    /// <param name="value">How much to reduce health by; set negative to heal.</param>
    void ModifyHealth(float value);
    /// <summary>
    /// Adds to maxHealthMod.
    /// </summary>
    /// <param name="value">how much to add to the max health; set negative to reduce.</param>
    void ModifyMaxHealth(float value);
    /// <summary>
    /// Resets maxHealthMod to zero.
    /// </summary>
    void ResetMaxHealth();
    /// <summary>
    /// Kills the object.
    /// </summary>
    void Kill();
}

/// <summary>
/// An interface for damage threshold effects.
/// the purpose of this is to allow methods to be called when certain HP thresholds are met.
/// </summary>
public interface IDamageThreshold
{
    [SerializeField]
    List<DamageThreshold> DamageThresholds { get; set; }
    void CheckDamageThresholds();
}

/// <summary>
/// An abstract scriptableObject that provides methods and variables for systems to interact with thresholds with
/// </summary>
[System.Serializable]
public abstract class DamageThreshold : ScriptableObject
{
    public abstract float LowThreshold { get; } // 0 - 1 percent
    public abstract float HighThreshold { get; } // 0 - 1 percent
    public abstract bool Active { get; set; } // is true when outside of the threshold, false when inside.
    public abstract void Start(); // runs on the first frame of being within the threshold
    public abstract void Action(); // what to do when within the threshold (this is not every frame, just every time its checked)
    public abstract void End(); // runs after exiting the threshold
}
