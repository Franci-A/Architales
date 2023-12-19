using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentManager : MonoBehaviour
{
    public static ResidentManager Instance;
    [SerializeField] private FloatVariable balanceMultiplier;
    [SerializeField] private FloatVariable maxBalanceAdded;
    [SerializeField] private IntVariable happinessGain;
    [SerializeField] private IntVariable numberHappyResidents;
    [SerializeField] private IntVariable totalNumResidents;
    [SerializeField] private GameplayDataSO gameplayData;
    private void Awake()
    {
        Instance = this;
        happinessGain.SetValue(0);
        numberHappyResidents.SetValue(0);
        totalNumResidents.SetValue(0);
        maxBalanceAdded.SetValue(0);
        balanceMultiplier.SetValue(1);
    }

    public void UpdateResidentsHappiness(int value)
    {
        happinessGain.Add(value);
        numberHappyResidents.Add(value);
        /*if(numberHappyResidents.value > 0)
        {
            float max = 0;
            for (int i = 0; i < gameplayData.residentHappinessLevels.Count; i++)
            {
                if(numberHappyResidents.value > gameplayData.residentHappinessLevels[i].numberOfResidents && gameplayData.residentHappinessLevels[i].maxBalanceAddedValue > max)
                {
                    max = gameplayData.residentHappinessLevels[i].maxBalanceAddedValue;
                }
            }
            maxBalanceAdded.SetValue(max);

        }
        else
        {
            maxBalanceAdded.SetValue(0);
        }


        if (numberHappyResidents.value > 0)
        {
            float multiplier = 1;
            for (int i = 0; i < gameplayData.residentAngryLevels.Count; i++)
            {
                if (gameplayData.residentAngryLevels[i].numberOfResidents < 0)
                    continue;
                if (numberHappyResidents.value >= gameplayData.residentAngryLevels[i].numberOfResidents && multiplier > gameplayData.residentAngryLevels[i].balanceMultiplier)
                {
                    multiplier = gameplayData.residentAngryLevels[i].balanceMultiplier;
                }
            }
            balanceMultiplier.SetValue(multiplier);

        }
        else if(numberHappyResidents.value < 0)
        {
            float multiplier = 1;
            for (int i = 0; i < gameplayData.residentAngryLevels.Count; i++)
            {
                if (gameplayData.residentAngryLevels[i].numberOfResidents > 0)
                    continue;
                if (numberHappyResidents.value <= gameplayData.residentAngryLevels[i].numberOfResidents && multiplier < gameplayData.residentAngryLevels[i].balanceMultiplier)
                {
                    multiplier = gameplayData.residentAngryLevels[i].balanceMultiplier;
                }
            }
            balanceMultiplier.SetValue(multiplier);
        }
        else
        {
            balanceMultiplier.SetValue(1);
        }*/
    }
}
