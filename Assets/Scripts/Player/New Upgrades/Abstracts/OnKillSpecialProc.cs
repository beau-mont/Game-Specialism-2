using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnKillSpecialProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnKillSpecial += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnKillSpecial -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}