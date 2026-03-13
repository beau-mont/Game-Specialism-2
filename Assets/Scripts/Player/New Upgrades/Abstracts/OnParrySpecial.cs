using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnParrySpecialProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnParrySpecial += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnParrySpecial -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}