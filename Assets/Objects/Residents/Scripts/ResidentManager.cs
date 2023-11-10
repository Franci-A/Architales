using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentManager : MonoBehaviour
{
    public static ResidentManager Instance;
    [SerializeField] private FloatVariable balanceMultiplier;
    [SerializeField] private IntVariable numberAngryResidents;
    [SerializeField] private IntVariable numberHappyResidents;
    [SerializeField] private IntVariable totalNumResidents;

    private void Awake()
    {
        Instance = this;
        numberAngryResidents.SetValue(0);
        numberHappyResidents.SetValue(0);
        totalNumResidents.SetValue(0);
    }

    public void UpdateHappyResidents(int value)
    {
        numberHappyResidents.Add(value);
    }
    
    public void UpdateAngryResidents(int value)
    {
        numberAngryResidents.Add(value);
    }
}
