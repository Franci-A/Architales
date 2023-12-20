using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameplayDataSO gameplayData;
    [SerializeField] private EventObjectScriptable onLastPiecePlaced;
    [SerializeField] private EventObjectScriptable onPiecePlaced;
    private int combo;
    private float score;
    [SerializeField] private FloatVariable scoreVariable;
    [SerializeField] private ScorePopupHandler scorePopup;
    [SerializeField] private IntVariable happinessGain;

    [Header("SFX")]
    [SerializeField] GameObject happySFX;
    [SerializeField] GameObject angrySFX;

    private void Start()
    {
        score = 0;
        combo = 0;

        onLastPiecePlaced.AddListener(OnLastPiecePlaced);
        onPiecePlaced.AddListener(OnScoreUpdated);
        scoreVariable.SetValue(0);
    }

    private void OnLastPiecePlaced(object obj)
    {
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
    }

    private void OnScoreUpdated(object obj)
    {
        var piece = obj as PieceHappinessHandler;

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
        // Reset Happiness OnPiecePlaced Event
        // AFTER Every call to LastPlacedPiece callbacks
        happinessGain.SetValue(0);
    }


    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(OnScoreUpdated);
        onLastPiecePlaced.RemoveListener(OnLastPiecePlaced);
    }
}
