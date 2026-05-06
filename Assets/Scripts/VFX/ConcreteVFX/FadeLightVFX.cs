using System;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// A VFXStrategy for fading out a spriteRenderer along an animation curve.
/// </summary>
public class FadeLightVFX : VFXStrategy
{
    private PayloadMultipliers _multipliers = new();
    public override PayloadMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }
    public float fadeTime;
    public bool loop = false;
    public AnimationCurve intensityCurve;
    public Gradient gradient;
    public Light2D light2D;
    private Color startColor;
    private float startIntensity;
    public override float StartTime { get; set; }
    void OnEnable()
    {
        StartTime = Time.time;
        if (light2D) 
        {
            startColor = light2D.color;
            startIntensity = light2D.intensity;
        }
    }

    void Update()
    {
        if (Time.time < StartTime) return;
        if (Time.time > StartTime + fadeTime && loop) StartTime = Time.time;
        light2D.color = gradient.Evaluate((Time.time - StartTime) / fadeTime);
        light2D.intensity = startIntensity * intensityCurve.Evaluate((Time.time - StartTime) / fadeTime);
    }

    void OnDisable()
    {
        light2D.color = startColor;
        light2D.intensity = startIntensity;
    }
}
