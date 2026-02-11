using UnityEngine;

public class SFX_Component : MonoBehaviour
{
    void OnEnable()
    {
        gameObject.name = "Active SFX";
    }
    void OnDisable()
    {
        gameObject.name = "Pooled SFX";
    }
}
