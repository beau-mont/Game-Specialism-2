using UnityEngine;

public class SimplePickup : MonoBehaviour
{
    public IAbility abilityToGrant;
    [SerializeField] private PlayerData playerData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (abilityToGrant != null)
            {
                playerData.Player.GetComponent<IAbilityUser>().AddAbility(abilityToGrant);
                Debug.Log($"Added ability: {abilityToGrant.AbilityName}");
                Dissapear(); // destroy pickup after granting ability
            }
            else
            {
                Debug.LogError($"{gameObject.name} has no abilityToGrant.");
            }
        }
    }

    private void Dissapear()
    {
        Destroy(gameObject); // replace with fancy animation or something later
    }
}
