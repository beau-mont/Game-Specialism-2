using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Config")]
    public GameObject Player;
    public IAbilityUser PlayerAbilityUser;
    [Header("Health")]
    public float MaxHealth;
    public float Health; // maybe do int for health?
    public int MaxLives;
    public int Lives; 
    [Header("Movement")]
    public float MaxSpeed;
    public float Acceleration;
    public Vector2 CurrentVelocity;

    public void Start()
    {
        Health = MaxHealth;
        Lives = MaxLives;
        CurrentVelocity = Vector2.zero;
    }
}