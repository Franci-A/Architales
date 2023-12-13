using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FeedbackPopup : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image image;
    [SerializeField] private Sprite happy;
    [SerializeField] private Sprite angry;
    [SerializeField] private Sprite neutral;
    [SerializeField] private float animTime = 1f;

    public void InitPopup(int likeAmount)
    {

        image.material.color = Color.white;
        if (likeAmount > 0) 
        { 
            image.sprite = happy; 
        }
        else if(likeAmount  == 0)
        {
            image.sprite = neutral;
        }
        else
        {
            image.sprite = angry;
        }
        image.color = Color.clear;
        canvas.transform.DOLocalMoveY(1, animTime);
        image.DOColor(Color.white, animTime);
    }

    public void DestroyPopup()
    {
        if (this == null || this.gameObject == null)
            return;

        if (DOTween.IsTweening(canvas.transform))
            canvas.transform.DOKill();
        canvas.transform.DOLocalMoveY(0, animTime / 2);


        if (DOTween.IsTweening(image))
            image.DOComplete();
        image.DOColor(Color.clear, animTime / 2);

        Destroy(gameObject, animTime / 2);
    }

    private void OnDestroy()
    {
        if (DOTween.IsTweening(canvas.transform))
            canvas.transform.DOKill();
        if (DOTween.IsTweening(image))
            image.DOKill();
    }
}
