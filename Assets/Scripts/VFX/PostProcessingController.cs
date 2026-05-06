using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class PostProcessingController : MonoBehaviour
{
    public Volume postProcessingVolume;
    public float bloomIntensity = 0f;
    public float bloomThreshold = 0.9f;
    public float abberationIntensity;
    public float LerpSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        postProcessingVolume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        postProcessingVolume.profile.TryGet(out Bloom bloom);
        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, bloomIntensity, Time.deltaTime * LerpSpeed);
        bloom.threshold.value = Mathf.Lerp(bloom.threshold.value, bloomThreshold, Time.deltaTime * LerpSpeed);

        postProcessingVolume.profile.TryGet(out ChromaticAberration aberration);
        aberration.intensity.value = Mathf.Lerp(aberration.intensity.value, abberationIntensity / 2f, Time.deltaTime * LerpSpeed);
    }
}
