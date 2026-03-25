using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnDamageProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnDamage += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnDamage -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}