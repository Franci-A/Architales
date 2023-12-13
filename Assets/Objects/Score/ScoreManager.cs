using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameplayDataSO gameplayData;
    [SerializeField] private EventObjectScriptable onPiecePlaced;
    private int combo;
    private float score;
    [SerializeField] private FloatVariable scoreVariable;

    private void Start()
    {
        score = 0;
        combo = 0;
        onPiecePlaced.AddListener(OnScoreUpdated);
        scoreVariable.SetValue(0);
    }

    private void OnScoreUpdated(object obj)
    {
        var piece = obj as PieceHappinessHandler;
        int happinessGain = piece.GetHappinessLevel();
        if (happinessGain > 0)
        {
            combo++;
            combo = Mathf.Clamp(combo, 0, gameplayData.maxCombo);
        }
        else if (happinessGain < 0)
        {
            combo = 0;
        }

        float valueAdded = 0;
        if(combo == 0 && happinessGain < 0)
        {
            score += Mathf.RoundToInt(gameplayData.baseScore * gameplayData.unhappyMultiplier);
            valueAdded = Mathf.RoundToInt(gameplayData.baseScore * gameplayData.unhappyMultiplier);
        }
        else if (happinessGain > 0)
        {
            score += gameplayData.baseScore * combo;
            valueAdded = gameplayData.baseScore * combo;

        }
        else
        {
            score += gameplayData.baseScore / 2;
            valueAdded = gameplayData.baseScore / 2;
        }

        Debug.Log("combo : " + combo + " value add : " + valueAdded);
        scoreVariable.SetValue(score);
    }


    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(OnScoreUpdated);
    }
}
