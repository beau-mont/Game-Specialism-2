using System;
using TMPro;
using UnityEngine;

public class DebugAbilityDisplay : MonoBehaviour
{
    public TextMeshProUGUI textObject;
    public PlayerData playerData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayUpgrades();
    }

    void DisplayUpgrades()
    {
        string upgradeText = "-----------------------------------------" + Environment.NewLine;
        foreach (var upgrade in playerData.PlayerUpgradeManager.obtainedUpgrades)
        {
            upgradeText += $"{upgrade.UpgradeName}" + Environment.NewLine + $"{upgrade.UpgradeDescription}" + Environment.NewLine + "-----------------------------------------" + Environment.NewLine;
        }
        textObject.text = upgradeText;
    }
}
