using System;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// this is a strategy for fading out a gameobject
/// since this is a strategy the client should create an instance of this.
/// </summary>
[CreateAssetMenu(fileName = "FadeConfig", menuName = "VFX/Fade")]
public class FadeVFX : VFXStrategy
{
    public AnimationCurve fadeCurve;
    public Gradient gradient;
    public override void Begin(VFX_Data args)
    {
        if (!args.sr)
        {
            Debug.LogError($"no sprite renderer detected on {args.User.name}");
            return;
        }
    }

    public override void Process(VFX_Data args)
    {
        if (!args.sr)
        {
            Debug.LogError($"no sprite renderer detected on {args.User.name}");
            return;
        }
        if (Time.time < args.StartTime) return;
        float fadeValue = fadeCurve.Evaluate((Time.time - args.StartTime) / fadeCurve.keys.Last().time);
        args.sr.color = gradient.Evaluate(fadeValue);
        //Debug.Log($"Start time: {args.StartTime}. End time: {args.StartTime + fadeCurve.keys.Last().time}. Current time: {Time.time}. Percent complete: {(Time.time - args.StartTime) / fadeCurve.keys.Last().time * 100}% Fade curve: {fadeCurve}.");
    }

    public override void End(VFX_Data args)
    {
        if (!args.sr)
        {
            Debug.LogError($"no sprite renderer detected on {args.User.name}");
            return;
        }
    }
}
