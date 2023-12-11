using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Data")]

public class GameplayDataSO : ScriptableObject
{
    [Header("Balance")]
    public FloatVariable balanceAddedVariable;
    public FloatVariable balanceMultiplierVariable;
    [SerializeField] private float maxBalance;
    
    [Header("Resident Happiness")]
    public List<ResidentHappinessLevel> residentHappinessLevels;
    public List<ResidentAngryLevel> residentAngryLevels;
    
    [Header("Resident Pieces")]
    [SerializeField] public int InitialPiecesNumber;
    [SerializeField] public int PositiveHappinessPieceGain;
    [SerializeField] public int NeutralHappinessPieceGain;
    [SerializeField] public int NegativeHappinessPieceGain;
    [SerializeField] public int ResidentsToLoseGame;

    public float MaxBalance => maxBalance + balanceAddedVariable;
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