using UnityEngine;

/// <summary>
/// Heals the player by healAmount when they kill any enemy
/// </summary>
public class KillHeal : OnKillProc
{
    public PlayerData playerData;
    public float healAmount = 0.1f;
    public override void ProcEffect()
    {
        if (!playerData)  
        {
            Debug.LogWarning($"no playerdata given to script on {gameObject.name}.");
            if (TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ModifyHealth(-healAmount);
            }
        }
        else
        {
            playerData.PlayerController.ModifyHealth(-healAmount);
        }
    }
}
