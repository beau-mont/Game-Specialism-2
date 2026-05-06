using UnityEngine;

/// <summary>
/// A simple pickup monoBehaviour, this is likely to be removed or reworked later for real functionality.
/// </summary>
public class SimplePickup : MonoBehaviour
{
    public Vector3 moveDir;
    public AbstractAbility  abilityToGrant;
    public Upgrade upgradeToAdd;
    [SerializeField] private PlayerData playerData;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (abilityToGrant)
            {
                playerData.PlayerAbilityUser.AddAbility(abilityToGrant);
                Disappear(); // destroy pickup after granting ability
            }
            if (upgradeToAdd)
            {
                playerData.PlayerUpgradeManager.AddUpgrade(upgradeToAdd);
                Disappear(); // destroy pickup after granting upgrade
            }
        }
    }

    private void Disappear()
    {
        Destroy(gameObject); // replace with fancy animation or something later
    }

    void Update()
    {
        transform.position += moveDir * Time.deltaTime;
    }
}
