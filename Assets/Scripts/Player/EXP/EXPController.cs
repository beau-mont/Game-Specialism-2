using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EXPController : MonoBehaviour
{
    [field: SerializeField] public int CurrentEXP { get; private set; }
    [field: SerializeField] public int PlayerLevel { get; private set; }
    [field: SerializeField] public int[] EXPThresholds { get; private set; }
    private int totalXPcollected;

    public UnityEvent<int> OnLevelUp;
    public UnityEvent<int> OnEXPChange;
    public GameObject display;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerLevel = 0;
        CurrentEXP = 0;
    }

    public void AddEXP(int amount)
    {
        CurrentEXP += amount;
        OnEXPChange?.Invoke(CurrentEXP);
        CheckLevelUp();
        totalXPcollected += amount;
        display.GetComponent<TextMeshProUGUI>().text = totalXPcollected.ToString();
    }

    private void CheckLevelUp()
    {
        if (CurrentEXP >= EXPThresholds[PlayerLevel])
        {
            PlayerLevel++;
            CurrentEXP -= EXPThresholds[PlayerLevel - 1];
            OnLevelUp?.Invoke(PlayerLevel);
        }
    }
}
