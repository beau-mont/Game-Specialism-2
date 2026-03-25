using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnParrySpecialProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnParrySpecial += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnParrySpecial -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}