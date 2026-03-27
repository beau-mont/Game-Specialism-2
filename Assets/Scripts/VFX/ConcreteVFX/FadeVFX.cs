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
public class FadeVFX : VFXStrategy
{
    private PayloadMultipliers _multipliers = new();
    public override PayloadMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }
    public float fadeTime;
    public Gradient gradient;
    public SpriteRenderer sr;
    private Color startColor;
    public override float StartTime { get; set; }
    void OnEnable()
    {
        StartTime = Time.time;
        if (sr) startColor = sr.color;
    }

    void Update()
    {
        if (Time.time < StartTime) return;
        sr.color = gradient.Evaluate((Time.time - StartTime) / fadeTime);
    }

    void OnDisable()
    {
        sr.color = startColor;
    }
}
