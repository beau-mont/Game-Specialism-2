using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnParryProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnParry += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnParry -= ProcEffect;
        }
    }

    public abstract void ProcEffect(bool wasSuccessful);
}