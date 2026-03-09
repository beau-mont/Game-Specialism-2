using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class OnHitUpgrade : MonoBehaviour
{
    public int Stacks { get; set; }
    protected void OnEnable()
    {
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.OnHit += OnHit;
        }
        else
        {
            Debug.LogError("No PlayerController found on " + gameObject.name);
            return;
        }
    }

    public abstract void OnHit(HitProcArgs args);
}

public class HitProcArgs
{
    public GameObject Target;
    public GameObject User;
}