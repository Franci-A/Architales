using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    [SerializeField] List<GameObject> list = new List<GameObject>();

    private int slideId;
    private int previousSlideId;

    public void NextSlide()
    {
        slideId++;
        ChangeSlide();
    }

    public void PrevSlide() 
    {
        slideId--;
        ChangeSlide();
    }

    void ChangeSlide()
    {
        list[previousSlideId].SetActive(false);
        list[slideId].SetActive(true);

        previousSlideId = slideId;
    }
}
