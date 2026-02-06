using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private IAbilityUser abilityUser;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!abilityUser) abilityUser = GetComponent<IAbilityUser>();
        if (!abilityUser)
        {
            Debug.LogWarning($"No abilityUser assigned to player object, creating instance to maintain functionality");
            abilityUser = gameObject.AddComponent<IAbilityUser>();
        }
        playerData.Start();
        playerData.Player = gameObject;
        playerData.PlayerAbilityUser = abilityUser;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Mouse1)) abilityUser.CycleAbility(); // TODO: Give ability switching proper bindings
        if (!abilityUser.currentAbility) abilityUser.CycleAbility();

        if (Input.GetKeyDown(KeyCode.Space)) // TODO: Replace all of this with how the player actually fires
        {
            abilityUser.currentAbility.ActivateAbility(abilityUser);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            abilityUser.currentAbility.HoldAbility(abilityUser);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            abilityUser.currentAbility.DeactivateAbility(abilityUser);
            if (abilityUser.currentAbility.IsSingleUse)
            {
                abilityUser.availableAbilities.RemoveAll(a => a == abilityUser.currentAbility);
                abilityUser.CycleAbility();
            }
        }
    }
}
