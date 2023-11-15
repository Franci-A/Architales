using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinessSliderUpdater : MonoBehaviour
{
    [SerializeField] private Slider happySlider;

    [SerializeField] private IntVariable numberHappyResidents;
    [SerializeField] private Image image;
    [SerializeField] private Sprite happyImage;
    [SerializeField] private Sprite angryImage;



    private void Start()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);
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
    }

    private void OnDestroy()
    {
        numberHappyResidents.OnValueChanged.AddListener(UpdateSliderValue);

    }
}
