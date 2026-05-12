using UnityEngine;

/// <summary>
/// A concrete implementation of a VFXStrategy, disables the entity after a set delay.
/// </summary>
public class DisableAfterDelayVFX : VFXStrategy
{
    private PayloadMultipliers _multipliers = new();
    public override PayloadMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }
    public float Delay;
    public override float StartTime { get; set; }
    private void OnEnable()
    {
        StartTime = Time.time;
    }

    void Update()
    {
        if (Time.time < StartTime) return;
        if (Time.time > StartTime + Delay)
        {
            // Debug.Log($"VFX Disabling {args.User.name}");
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        
    }
}
