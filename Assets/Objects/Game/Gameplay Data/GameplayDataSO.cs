using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Data")]

public class GameplayDataSO : ScriptableObject
{
    [SerializeField] private float maxBalance;
    public FloatVariable balanceAddedVariable;
    public float MaxBalance => maxBalance + balanceAddedVariable;
    public List<ResidentHappinessLevel> residentHappinessLevels;
    public List<ResidentAngryLevel> residentAngryLevels;
    public FloatVariable balanceMultiplierVariable;
}

[Serializable]
public struct ResidentHappinessLevel
{
    public int numberOfResidents;
    public float maxBalanceAddedValue;
}

[Serializable]
public struct ResidentAngryLevel
{
    public int numberOfResidents;
    public float balanceMultiplier;
}