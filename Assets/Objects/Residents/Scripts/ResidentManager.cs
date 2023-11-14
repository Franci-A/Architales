using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentManager : MonoBehaviour
{
    public static ResidentManager Instance;
    [SerializeField] private FloatVariable balanceMultiplier;
    [SerializeField] private IntVariable numberHappyResidents;
    [SerializeField] private IntVariable totalNumResidents;

    private void Awake()
    {
        Instance = this;
        numberHappyResidents.SetValue(0);
        totalNumResidents.SetValue(0);
    }

    public void UpdateResidentsHappiness(int value)
    {
        numberHappyResidents.Add(value);
        //if > 0 then add to resistance
        //else if < 0 add to multiplier
    }
}
