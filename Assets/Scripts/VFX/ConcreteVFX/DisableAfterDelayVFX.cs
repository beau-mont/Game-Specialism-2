using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

/// <summary>
/// A concrete implementation of a VFXStrategy, disables the entity after a set delay.
/// </summary>
public class DisableAfterDelayVFX : VFXStrategy
{
    private PayloadMultipliers _multipliers = new();
    public override PayloadMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }
    public float Delay;
    private float startTime;
    private void OnEnable()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time < startTime) return;
        if (Time.time > startTime + Delay)
        {
            // Debug.Log($"VFX Disabling {args.User.name}");
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        
    }
}
