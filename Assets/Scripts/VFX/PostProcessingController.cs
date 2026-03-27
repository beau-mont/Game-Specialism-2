using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class PostProcessingController : MonoBehaviour
{
    public Volume postProcessingVolume;
    public float bloomIntensity = 0f;
    public float bloomThreshold = 0.9f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        postProcessingVolume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        postProcessingVolume.profile.TryGet(out Bloom bloom);
        bloom.intensity.value = bloomIntensity;
        bloom.threshold.value = bloomThreshold;
    }
}
