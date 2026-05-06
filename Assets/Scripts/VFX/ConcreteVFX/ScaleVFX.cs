using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A VFXStrategy to scale an object along an animation curve.
/// </summary>
public class ScaleVFX : VFXStrategy
{
    private PayloadMultipliers _multipliers = new();
    public override PayloadMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }
    public AnimationCurve xScaleCurve;
    public AnimationCurve yScaleCurve;
    public AnimationCurve zScaleCurve;
    private Vector3 startScale;
    public override float StartTime { get; set; }
    void OnEnable()
    {
        StartTime = Time.time;
        startScale = transform.localScale;
    }

    void Update()
    {
        //if (Time.time < StartTime) return;
        float mult = 1 + Multipliers.GlobalMultiplier + Multipliers.RadiusMultiplier;
        float xScaleValue = startScale.x * (xScaleCurve.Evaluate(Time.time - StartTime) * mult);
        float yScaleValue = startScale.y * (yScaleCurve.Evaluate(Time.time - StartTime) * mult);
        float zScaleValue = startScale.z * (zScaleCurve.Evaluate(Time.time - StartTime) * mult);
        //Debug.Log($"Time: {(Time.time - StartTime) / zScaleCurve.keys.Last().time}. Scale: {new Vector3(xScaleValue, yScaleValue, zScaleValue)}. mult: {mult}.");
        transform.localScale = new Vector3(xScaleValue, yScaleValue, zScaleValue);
    }

    void OnDisable()
    {
        transform.localScale = startScale;
    }
}
