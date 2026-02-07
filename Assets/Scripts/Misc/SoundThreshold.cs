using UnityEngine;

[CreateAssetMenu(fileName = "SoundPlayer", menuName = "Player/DamageThresholds/SoundPlayer")]
public class SoundThreshold : DamageThreshold // todo: COMPLETELY REWORK THIS, ITS JUST AN EXAMPLE
{
    [SerializeField] private float _lowThreshold;
    [SerializeField] private float _highThreshold;
    public override float LowThreshold { get => _lowThreshold; } // 0 - 1 percent
    public override float HighThreshold { get => _highThreshold; } // 0 - 1 percent
    public override bool Active { get; set; } // is true when outside of the threshold, false when inside.
    public AudioClip audioClip;
    public AudioSource source;
    public override void Start() // runs on the first frame of being within the threshold 
    {
        Debug.Log("start");
        source = FindFirstObjectByType<AudioSource>();
        if (audioClip && source)
        {
            source.clip = audioClip;
            source.loop = true;
            source.Play();
        }
    }
    public override void Action() // what to do when within the threshold
    {
        Debug.Log("action");
    }
    public override void End() // runs after exiting the threshold
    {
        if (source)
        {
            source.Stop();
        }
        Debug.Log("end");
    }
}
