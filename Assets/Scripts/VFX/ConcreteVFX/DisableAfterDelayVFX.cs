using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "DisableAfterDelayConfig", menuName = "VFX/DisableAfterDelay")]
public class DisableAfterDelayVFX : VFXStrategy
{
    public float Delay;
    public override void Begin(VFX_Data args)
    {
        
    }

    public override void Process(VFX_Data args)
    {
        if (Time.time < args.StartTime) return;
        if (Time.time > args.StartTime + Delay)
        {
            // Debug.Log($"VFX Disabling {args.User.name}");
            args.User.SetActive(false);
        }
    }

    public override void End(VFX_Data args)
    {
        
    }
}
