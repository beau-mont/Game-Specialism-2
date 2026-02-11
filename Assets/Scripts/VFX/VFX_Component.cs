using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a monoBehavior that executes a list of VFXStrategies it is provided.
/// </summary>
public class VFX_Component : MonoBehaviour // TODO: Make a bunch of these private/static/const
{
    [SerializeField] private List<VFXStrategy> strategies;
    public PayloadMultipliers multipliers = new();
    private VFX_Data VFXData = new();
    private bool init = false;

    void OnEnable()
    {
        if (!init)
        {
            VFXData.User = gameObject;
            VFXData.sr = GetComponent<SpriteRenderer>();
            VFXData.Scale = transform.localScale;
            init = true;
        }
        VFXData.StartTime = Time.time;
        if (VFXData.StartTime <= 0) VFXData.StartTime = 0.1f;
        VFXData.Multipliers = multipliers;
        foreach (var strategy in strategies)
        {
            strategy.Begin(VFXData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var strategy in strategies)
        {
            strategy.Process(VFXData);
        }
    }

    void OnDisable()
    {
        foreach (var strategy in strategies)
        {
            strategy.End(VFXData);
        }
    }
}

/// <summary>
/// instead of passing a ton of bullshit to the scriptable objects, create a data container class
/// populate on Start/OnEnable to minimise operations and then pass to strategies
/// this means we dont need to create instances of scriptable objects which can be troublesome to handle without causing memory leaks
/// </summary>
public class VFX_Data
{
    public GameObject User;
    public float StartTime;
    public SpriteRenderer sr;
    public Vector3 Scale;
    public PayloadMultipliers Multipliers;
}

/// <summary>
/// Abstract class defining VFXStrategy methods.
/// TODO: make this an interface(?)
/// </summary>
[System.Serializable]
public abstract class VFXStrategy : ScriptableObject
{
    /// <summary>
    /// Set the starting conditions for the VFX
    /// </summary>
    /// <param name="args">VFX_data, update the class when you need to move more information here.</param>
    public abstract void Begin(VFX_Data args);
    /// <summary>
    /// Process the VFX
    /// </summary>
    /// <param name="args">VFX_data, update the class when you need to move more information here.</param>
    public abstract void Process(VFX_Data args);
    /// <summary>
    /// Reset the VFX object to starting condition
    /// </summary>
    /// <param name="args">VFX_data, update the class when you need to move more information here.</param>
    public abstract void End(VFX_Data args);
}

