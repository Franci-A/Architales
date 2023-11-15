using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackPopup : MonoBehaviour
{
    [SerializeField] private Renderer plane;
    [SerializeField] private Material happy;
    [SerializeField] private Material angry;


    public void InitPopup(bool isPositive)
    {
        if(isPositive) 
        { 
            plane.material = happy; 
        }
        else
        {
            plane.material = angry;
        }
    }
}
