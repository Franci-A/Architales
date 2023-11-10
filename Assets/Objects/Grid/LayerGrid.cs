using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerGrid : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    [SerializeField] Slider layerSlider;

    private void Start()
    {
        Grid3DManager.Instance.OnLayerCubeChange += UpdateSliderMaximum;
    }

    void UpdateSliderMaximum(int higherValue)
    {
        if (higherValue < 1) return;

        var actualPourcentage =  layerSlider.value / layerSlider.maxValue;
        layerSlider.maxValue = higherValue;

        if (actualPourcentage == 1)
            layerSlider.value = layerSlider.maxValue;
        else
            gridData.HideBlocksAtHeight((int)layerSlider.value);
    }

    public void OnValueChanged(float layerLevel)
    {
        if (layerLevel < 1) return;

        gridData.HideBlocksAtHeight((int)layerLevel);
    }
}
