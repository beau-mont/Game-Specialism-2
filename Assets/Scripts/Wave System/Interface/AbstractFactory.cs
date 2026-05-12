using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract factory class
/// </summary>
public abstract class IWaveFactory : ScriptableObject // abstract factory class
{
    public abstract List<GameObject> CreateWave(WaveData waveData);
    public abstract string FactoryName { get; }
    public abstract float MaxHealthMod { get; set; }
}