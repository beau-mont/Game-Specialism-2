using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFX_Component : MonoBehaviour
{
    public AudioSource source;
    public bool disableWhenNotPlaying = true;
    public bool useRandom = false;
    public Vector2 randVolume;
    public Vector2 randPitch;
    public Vector2 randPan;
    void OnEnable()
    {
        gameObject.name = "Active SFX";
        source.Play();

        if (!source) source = GetComponent<AudioSource>();
        
        if (useRandom) RandomiseSource();
    }

    private void RandomiseSource()
    {
        source.volume = Random.Range(randVolume.x, randVolume.y);
        source.pitch = Random.Range(randPitch.x, randPitch.y);
        source.panStereo = Random.Range(randPan.x, randPan.y);
    }

    void Update()
    {
        if (!source.isPlaying && disableWhenNotPlaying)
        {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        gameObject.name = "Pooled SFX";
    }
}
