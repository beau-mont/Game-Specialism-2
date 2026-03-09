using System;
using System.Linq;
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
    private float startTime;
    void OnEnable()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        startTime = Time.time;
        startColor = sr.color;
    }

    void Update()
    {
        if (!sr)
        {
            Debug.LogError($"no sprite renderer detected on {gameObject.name}");
            return;
        }
        if (Time.time < startTime) return;
        sr.color = gradient.Evaluate((Time.time - startTime) / fadeTime);
    }

    void OnDisable()
    {
        sr.color = startColor;
    }
}
