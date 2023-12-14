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
    [SerializeField] private ScorePopupHandler scorePopup;

    [Header("SFX")]
    [SerializeField] GameObject happySFX;
    [SerializeField] GameObject angrySFX;

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
        string comboStr = "1";
        if (happinessGain > 0)
        {
            combo++;
            combo = Mathf.Clamp(combo, 0, gameplayData.maxCombo);
            Instantiate(happySFX);
        }
        else if (happinessGain < 0)
        {
            combo = 0;
            Instantiate(angrySFX);
        }

        float valueAdded = 0;
        if(combo == 0 && happinessGain < 0)
        {
            score += Mathf.RoundToInt(gameplayData.baseScore * gameplayData.unhappyMultiplier);
            valueAdded = Mathf.RoundToInt(gameplayData.baseScore * gameplayData.unhappyMultiplier);
            Instantiate<ScorePopupHandler>(scorePopup, piece.transform.position, Quaternion.identity).Init("" + valueAdded, combo);
        }
        else if (happinessGain > 0)
        {
            score += gameplayData.baseScore * combo;
            valueAdded = gameplayData.baseScore * combo;
            Instantiate<ScorePopupHandler>(scorePopup, piece.transform.position, Quaternion.identity).Init(gameplayData.baseScore + " x" + combo, combo);

        }
        else
        {
            score += gameplayData.baseScore / 2;
            valueAdded = gameplayData.baseScore / 2;
            Instantiate<ScorePopupHandler>(scorePopup, piece.transform.position, Quaternion.identity).Init("" + valueAdded, combo);

        }

        scoreVariable.SetValue(score);
    }


    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(OnScoreUpdated);
    }
}
