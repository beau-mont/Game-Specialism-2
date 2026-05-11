using UnityEngine;
using UnityEngine.UI;

public class KillSpecialDisplay : MonoBehaviour
{
    public Image image;
    public float lerpSpeed;
    public float targetFill;

    // Update is called once per frame
    void Update()
    {
        if (!image) return;
        image.fillAmount = Mathf.Lerp(image.fillAmount, targetFill, Time.deltaTime * lerpSpeed);
    }
}
