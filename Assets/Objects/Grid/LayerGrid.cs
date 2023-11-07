using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerGrid : MonoBehaviour
{
    [SerializeField] Slider layerSlider;

    private void Start()
    {
        Grid3DManager.Instance.OnHigerCubeChange += UpdateSliderMaximum;
    }

    void UpdateSliderMaximum(int higherValue)
    {
        if (higherValue <= 1) return;

        var actualPourcentage =  layerSlider.value / layerSlider.maxValue;
        layerSlider.maxValue = higherValue - 1;
        layerSlider.value = higherValue * actualPourcentage;

    }

    public void OnValueChanged(float layerLevel)
    {
        Grid3DManager.Instance.ShowLayer((int)layerLevel);
    }


}
