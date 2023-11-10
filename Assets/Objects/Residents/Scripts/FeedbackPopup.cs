using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackPopup : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite happy;
    [SerializeField] private Sprite angry;


    public void InitPopup(bool isPositive)
    {
        if(isPositive) 
        { 
            image.sprite = happy;
        }
        else
        {
            image.sprite = angry;
        }
    }
}
