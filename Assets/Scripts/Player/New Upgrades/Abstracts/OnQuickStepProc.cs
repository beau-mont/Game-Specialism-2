using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnQuickStepProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnQuickStep += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnQuickStep -= ProcEffect;
        }
    }

    public abstract void ProcEffect();
}