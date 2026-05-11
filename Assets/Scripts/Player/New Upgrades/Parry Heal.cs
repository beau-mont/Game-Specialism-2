using UnityEngine;

/// <summary>
/// heals the player by healAmount when the player successfully parries an attack
/// </summary>
public class ParryHeal : OnParryProc 
{
    public PlayerData playerData;
    public float healAmount = 1;
    public override void ProcEffect(bool wasSuccessful)
    {
        if (!wasSuccessful) return;

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
