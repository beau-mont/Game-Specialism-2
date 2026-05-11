using UnityEngine;
using UnityEngine.Audio;

public class SetMixer : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMaster(float value)
    {
        mixer.SetFloat("Master", value);
    }

    public void SetMusic(float value)
    {
        mixer.SetFloat("Music", value);
    }

    public void SetSFX(float value)
    {
        mixer.SetFloat("SFX", value);
    }
}
