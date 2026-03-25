using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnKillProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnKill += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnKill -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}