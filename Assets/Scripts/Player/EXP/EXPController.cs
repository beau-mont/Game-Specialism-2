using System;
using UnityEngine;
using UnityEngine.Events;

public class EXPController : MonoBehaviour
{
    [field: SerializeField] public int CurrentEXP { get; private set; }
    [field: SerializeField] public int PlayerLevel { get; private set; }
    [field: SerializeField] public int[] EXPThresholds { get; private set; }

    public UnityEvent<int> OnLevelUp;
    public UnityEvent<int> OnEXPChange;
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
