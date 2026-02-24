using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a monoBehavior that executes a list of VFXStrategies it is provided.
/// </summary>
public class VFXComponent : MonoBehaviour
{
    [SerializeField] private VFXStrategy[] strategies;
    public PayloadMultipliers multipliers = new();

    void OnEnable()
    {
        foreach (var strategy in strategies)
        {
            strategy.Multipliers = multipliers;
        }
    }
}


/// <summary>
/// Abstract class defining VFXStrategy methods.
/// </summary>
public abstract class VFXStrategy : MonoBehaviour
{
    public abstract PayloadMultipliers Multipliers { get; set; }
}

