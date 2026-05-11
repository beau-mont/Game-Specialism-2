using UnityEngine;
using UnityEngine.InputSystem;

public class KillSpecialController : MonoBehaviour
{
    public PlayerData playerData;
    public int maxCharge;
    public int charge;
    InputAction switchAction;
    InputAction parryAction;

    void OnEnable() // lowkenuinely hyjacking the proc system to count kills cause im lazy
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnKill += AddKill;
        }
    }

    void OnDisable()
    {
        if (TryGetComponent<PlayerEventController>(out var eventController))
        {
            eventController.OnKill -= AddKill;
        }
    }

    void AddKill()
    {
        charge++;
        Debug.Log($"KILL PROC");
        FindFirstObjectByType<KillSpecialDisplay>().targetFill = Mathf.Clamp(charge / maxCharge, 0, 1);
    }

    void Start()
    {        
        switchAction = InputSystem.actions.FindAction("Switch");
        parryAction = InputSystem.actions.FindAction("Parry");
    }

    // Update is called once per frame
    void Update()
    {
        if (charge < maxCharge) return;

        // activate kill special if both are pressed
        if (switchAction.IsPressed() && parryAction.IsPressed()) 
        {
            playerData.Player.GetComponent<PlayerAbilityUser>().ActivateKillSpecial();
            charge = 0;
        }
    }
}
