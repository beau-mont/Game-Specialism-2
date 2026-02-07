using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Data about the player that other objects may need to access. there should only be one instance of this ever.
/// TODO: rework with the new IDamageable interface.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Config")]
    public GameObject Player;
    public IAbilityUser PlayerAbilityUser;
    public PlayerController PlayerController;
}