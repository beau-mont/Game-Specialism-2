using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeReader : MonoBehaviour
{
    public AudioMixer mixer;
    public string group;
    public TextMeshProUGUI textBox;

    // Update is called once per frame
    void Update()
    {
        if (!mixer || group == null || !textBox)
        {
            Debug.LogError($"missing somethingorother on the {gameObject.name}.");
            return;
        }

        mixer.GetFloat(group, out float value);
        textBox.text = value.ToString();
    }
}
