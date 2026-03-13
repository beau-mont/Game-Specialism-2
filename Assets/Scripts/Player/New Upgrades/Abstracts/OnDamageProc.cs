using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnDamageProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnDamage += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnDamage -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}