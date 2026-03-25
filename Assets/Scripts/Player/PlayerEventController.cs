using UnityEngine;
using UnityEngine.Events;

public class PlayerEventController : MonoBehaviour
{
    public PlayerData playerData;
    public UnityAction OnDamage;
    public UnityAction<bool> OnParry; // bool is true if parry was successful, false if it missed.
    public UnityAction OnKillSpecial;
    public UnityAction OnParrySpecial;
    public UnityAction OnQuickStep;
    public UnityAction OnKill;
}
