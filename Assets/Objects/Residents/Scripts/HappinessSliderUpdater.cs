using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinessSliderUpdater : MonoBehaviour
{
    [SerializeField] private Slider happySlider;

    [SerializeField] private IntVariable numberHappyResidents;



    private void Start()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);
    }

    public void UpdateSliderValue()
    {
        happySlider.value = numberHappyResidents.value;
    }

    private void OnDestroy()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);

    }
}
