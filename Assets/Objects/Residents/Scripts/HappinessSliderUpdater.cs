using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessSliderUpdater : MonoBehaviour
{
    [SerializeField] private Slider happySlider;

    [SerializeField] private IntVariable numberHappyResidents;
    [SerializeField] private Image image;
    [SerializeField] private Sprite happyImage;
    [SerializeField] private Sprite angryImage;
    [SerializeField] private TextMeshProUGUI currentNumberText;
    [SerializeField] private GameplayDataSO gameplayData;


    private void Start()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);
        happySlider.minValue = - gameplayData.residentAngryLevels.OrderByDescending(x => x.numberOfResidents).First().numberOfResidents;
        happySlider.maxValue = gameplayData.residentHappinessLevels.OrderByDescending(x => x.numberOfResidents).First().numberOfResidents;
        UpdateSliderValue();
    }

    public void UpdateSliderValue()
    {
        happySlider.value = numberHappyResidents.value;
        if(happySlider.value >= 0)
        {
            image.sprite = happyImage;
        }else
        {
            image.sprite = angryImage;
        }
        currentNumberText.text = numberHappyResidents.value.ToString();
    }

    private void OnDestroy()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);

    }
}
