using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinessSliderUpdater : MonoBehaviour
{
    [SerializeField] private Slider happySlider;
    [SerializeField] private Slider angrySlider;

    [SerializeField] private IntVariable numberAngryResidents;
    [SerializeField] private IntVariable numberHappyResidents;

    [SerializeField] private IntVariable totalResidents;


    private void Start()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);
        numberAngryResidents.OnValueChanged.AddListener(UpdateSliderValue);
        totalResidents.OnValueChanged.AddListener(UpdateSliderValue);
    }

    public void UpdateSliderValue()
    {
        happySlider.value = numberHappyResidents.value * 1.0f / totalResidents.value * 1.0f;
        angrySlider.value = numberAngryResidents.value * 1.0f / totalResidents.value * 1.0f;
    }

    private void OnDestroy()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);
        numberAngryResidents.OnValueChanged.AddListener(UpdateSliderValue);
        totalResidents.OnValueChanged.RemoveListener(UpdateSliderValue);

    }
}
