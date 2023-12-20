using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    [SerializeField] List<GameObject> list = new List<GameObject>();

    private int slideId;
    private int previousSlideId;

    private void Start()
    {
        Debug.Log("tuto spawned");
    }

    private void OnDestroy()
    {
        Debug.Log("tuto ded"); 
    }
    public void NextSlide()
    {
        slideId++;
        if(slideId >= list.Count) EndTuto();
        else ChangeSlide();
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

    void EndTuto()
    {
        GameManager.Instance.EndTuto();
        Debug.Log("endtuto");
        Destroy(gameObject);
    }
}
