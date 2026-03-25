using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnParryProc : MonoBehaviour
{
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnParry += ProcEffect;
        }
    }

    protected void OnDisable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnParry -= ProcEffect;
        }
    }

    public abstract void ProcEffect(bool wasSuccessful);
}