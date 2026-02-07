using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; } // todo: this variable doesn't need to be exposed
    void ModifyHealth(float health);
    void Kill();
}

public interface IDamageThreshold
{
    [SerializeField]
    List<DamageThreshold> DamageThresholds { get; set; }
    void CheckDamageThresholds();
}

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
