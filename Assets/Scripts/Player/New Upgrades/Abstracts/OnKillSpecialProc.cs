using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnKillSpecialProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnKill += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnKill -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}