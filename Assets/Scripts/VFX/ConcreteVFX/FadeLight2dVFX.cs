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
public class FadeLight2dVFX : VFXStrategy
{
    private PayloadMultipliers _multipliers = new();
    public override PayloadMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }
    public float fadeTime;
    public bool loop = false;
    public Gradient gradient;
    public Light2D light2d;
    private Color startColor;
    private float startTime;
    void OnEnable()
    {
        startTime = Time.time;
        if (light2d) startColor = light2d.color;
    }

    void Update()
    {
        if (Time.time < startTime) return;
        if (Time.time > startTime + fadeTime && loop) startTime = Time.time;
        light2d.color = gradient.Evaluate((Time.time - startTime) / fadeTime);
    }

    void OnDisable()
    {
        light2d.color = startColor;
    }
}
