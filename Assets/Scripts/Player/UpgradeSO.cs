using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Player/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string UpgradeName;
    public string UpgradeDescription;
    public PlayerMultipliers AddMultipliers;
}