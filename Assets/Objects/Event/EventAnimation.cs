using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private float targetLerp;
    private float targetLerpScale;

    [SerializeField] private GameObject crystalBall;
    [SerializeField] private GameObject eventText;


    private bool startSlide = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        StartUI();

        if (startSlide)
            Slide();

        
    }

    private void StartUI()
    {
        if (canvasGroup.alpha < 1f)
            canvasGroup.alpha += 0.01f;
        else
        {
            //start = false;
            StartCoroutine(Animation());
        }
    }

    IEnumerator Animation()
    {
        yield return new WaitForSeconds(3);
        startSlide = true;
    }

    private void Slide()
    {
        

        if (targetLerp < 1)
        {
            targetLerp += Time.deltaTime * 0.1f;
            targetLerpScale += Time.deltaTime;
        }
        else
        {
            startSlide = false;
            targetLerp = 0;
        }

        var minPos = eventText.transform.position;
        var maxPos = crystalBall.transform.position;

        var targetPosition = Vector3.Lerp(minPos, maxPos, targetLerp);
        eventText.transform.position = targetPosition;

        var targetScale = Mathf.Lerp(0, 1, 1 - targetLerpScale);
        eventText.transform.localScale = new Vector3 (targetScale, targetScale, 0);
    }
}
