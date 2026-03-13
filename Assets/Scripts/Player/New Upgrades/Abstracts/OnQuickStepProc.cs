using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnQuickStepProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnQuickStep += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnQuickStep -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}