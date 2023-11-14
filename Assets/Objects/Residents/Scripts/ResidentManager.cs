using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentManager : MonoBehaviour
{
    public static ResidentManager Instance;
    [SerializeField] private FloatVariable balanceMultiplier;
    [SerializeField] private FloatVariable maxBalanceAdded;
    [SerializeField] private IntVariable numberHappyResidents;
    [SerializeField] private IntVariable totalNumResidents;
    [SerializeField] private GameplayDataSO gameplayData;
    private void Awake()
    {
        Instance = this;
        numberHappyResidents.SetValue(0);
        totalNumResidents.SetValue(0);
    }

    public void UpdateResidentsHappiness(int value)
    {
        numberHappyResidents.Add(value);
        if(numberHappyResidents.value > 0)
        {
            bool hasValue = false;
            for (int i = 0; i < gameplayData.residentHappinessLevels.Count; i++)
            {
                if(numberHappyResidents.value > gameplayData.residentHappinessLevels[i].numberOfResidents)
                {
                    maxBalanceAdded.SetValue(gameplayData.residentHappinessLevels[i].maxBalanceAddedValue);
                    hasValue = true;
                }
            }
            if (!hasValue)
            {
                maxBalanceAdded.SetValue(0);
            }
        }else if (numberHappyResidents.value < 0)
        {
            bool hasValue = false;
            for (int i = 0; i < gameplayData.residentAngryLevels.Count; i++)
            {
                if (numberHappyResidents.value < gameplayData.residentAngryLevels[i].numberOfResidents)
                {
                    balanceMultiplier.SetValue(gameplayData.residentAngryLevels[i].balanceMultiplier);
                    hasValue = true;
                }
            }
            if (!hasValue)
            {
                balanceMultiplier.SetValue(1);
            }
        }else
        {
            maxBalanceAdded.SetValue(0);
            balanceMultiplier.SetValue(1);
        }
    }
}
