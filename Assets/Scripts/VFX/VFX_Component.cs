using System;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Component : MonoBehaviour
{
    [SerializeField] public List<VFXStrategy> strategies;
    public float modifier;
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
        VFXData.Modifier = modifier;
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
    public float Modifier;
}