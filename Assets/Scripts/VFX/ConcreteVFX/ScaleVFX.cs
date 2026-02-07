using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A VFXStrategy to scale an object along an animation curve.
/// </summary>
[CreateAssetMenu(fileName = "ScaleConfig", menuName = "VFX/Scale")]
public class ScaleVFX : VFXStrategy
{
    public AnimationCurve ScaleCurve;
    public override void Begin(VFX_Data args)
    {
        args.User.transform.localScale = args.Scale;
    }

    public override void Process(VFX_Data args)
    {
        if (Time.time < args.StartTime) return;
        float scaleValue = ScaleCurve.Evaluate((Time.time - args.StartTime) / ScaleCurve.keys.Last().time) * args.Modifier;
        args.User.transform.localScale = args.Scale * scaleValue;
    }

    public override void End(VFX_Data args)
    {
        args.User.transform.localScale = args.Scale;
    }
}
