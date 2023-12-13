using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreHandler : MonoBehaviour
{
    [SerializeField] private FloatVariable scoreVariable;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreVariable.OnValueChanged.AddListener(UpdateScoreUI);
        if(scoreText ==null)
            scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = scoreVariable.value.ToString();
    }

    private void OnDestroy()
    {
        scoreVariable.OnValueChanged.RemoveListener(UpdateScoreUI);

    }
}
