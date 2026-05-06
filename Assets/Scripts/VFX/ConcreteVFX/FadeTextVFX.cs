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
public class FadeTextVFX : VFXStrategy
{
    private PayloadMultipliers _multipliers = new();
    public override PayloadMultipliers Multipliers { get => _multipliers; set => _multipliers = value; }
    public float fadeTime;
    public bool loop = false;
    public Gradient gradient;
    public TextMeshProUGUI text;
    private Color textStartColor;
    public override float StartTime { get; set; }
    void OnEnable()
    {
        StartTime = Time.time;
        if (text) textStartColor = text.color;
    }

    void Update()
    {
        if (Time.time < StartTime) return;
        if (Time.time > StartTime + fadeTime && loop) StartTime = Time.time;
        text.color = gradient.Evaluate((Time.time - StartTime) / fadeTime);
    }

    void OnDisable()
    {
        text.color = textStartColor;
    }
}
