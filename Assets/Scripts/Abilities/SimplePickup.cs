using UnityEngine;

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
                Debug.Log($"sent ability: {abilityToGrant.AbilityName}");
                playerData.AddAbility(abilityToGrant);
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
