using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    public float FullPosX;
    public float EmptyPosX;
    private float targetX;
    public float LerpSpeed = 10f;
    public RectTransform rectTransform;
    
    public void SetHealthDisplay(float healthPercent)
    {
        targetX = Mathf.Lerp(EmptyPosX, FullPosX, healthPercent);
    }

    private void Update()
    {
        float newX = Mathf.Lerp(rectTransform.localPosition.x, targetX, Time.deltaTime * LerpSpeed);
        rectTransform.localPosition = new Vector3(newX, rectTransform.localPosition.y, rectTransform.localPosition.z);
    }
}

