using UnityEngine;

/// <summary>
/// A simple pickup monoBehaviour, this is likely to be removed or reworked later for real functionality.
/// </summary>
public class SimplePickup : MonoBehaviour
{
    public IAbility abilityToGrant;
    [SerializeField] private PlayerData playerData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (abilityToGrant)
            {
                // Debug.Log($"sent ability: {abilityToGrant.AbilityName}");
                playerData.PlayerAbilityUser.AddAbility(abilityToGrant);
                Disappear(); // destroy pickup after granting ability
            }
            else
            {
                Debug.LogError($"{gameObject.name} has no abilityToGrant.");
            }
        }
    }

    private void Disappear()
    {
        Destroy(gameObject); // replace with fancy animation or something later
    }
}
