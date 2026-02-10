using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private UpgradeTree[] availableTrees;
    public List<Upgrade> obtainedUpgrades;
    public PlayerData playerData;
    public PlayerMultipliers PlayerMultipliers;

    void Start()
    {
        playerData.PlayerUpgradeManager = this;
        // PlayerMultipliers = new PlayerMultipliers 
        // {
        //     GlobalMultiplier = 0f, 
        //     MoveSpeedMultiplier = 0f, 
        //     AbilityMultipliers = new AbilityMultipliers
        //     {
        //         GlobalMultiplier = 0f,
        //         AccuracyMultiplier = 0f,
        //         CooldownMultiplier = 0f,
        //         BonusProjectiles = 0,
        //         PayloadMultipliers = new PayloadMultipliers
        //         {
        //             GlobalMultiplier = 0f,
        //             DamageMultiplier = 0f,
        //             RadiusMultiplier = 0f,
        //             BurnMultiplier = 0f
        //         },
        //         AbilityBehaviourMultipliers = new AbilityBehaviourMultipliers
        //         {
        //             GlobalMultiplier = 0f,
        //             SpeedMultiplier = 0f,
        //             HomingMultiplier = 0f
        //         }
        //     }
        // };
        foreach (var upgrade in obtainedUpgrades)
        {
            PlayerMultipliers += upgrade.AddMultipliers;
        }
    }

    public void AddUpgrade(Upgrade upgrade) 
    {
        if (obtainedUpgrades.Contains(upgrade)) Debug.LogWarning($"Adding duplicate {upgrade.UpgradeName} to obtainedUpgrades"); 
        obtainedUpgrades.Add(upgrade); // allow for duplicates but log when adding them so we can track potentially unwanted behaviors.
        PlayerMultipliers += upgrade.AddMultipliers;
    }

    public void RemoveUpgrade(Upgrade upgrade)
    {
        if (obtainedUpgrades.Contains(upgrade)) Debug.LogWarning($"Removing duplicate {upgrade.UpgradeName} from obtainedUpgrades"); 
        obtainedUpgrades.Remove(obtainedUpgrades.FirstOrDefault(a => a == upgrade)); // allow for duplicates but log when removing them so we can track potentially unwanted behaviors.
        PlayerMultipliers -= upgrade.AddMultipliers;
    }
}

[System.Serializable]
public class UpgradeTree
{
    public string TreeName;
    public string TreeDescription;
    public Upgrade[] Upgrades;
}