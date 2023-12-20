using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{
    [Header("Position")]
    private CanvasGroup canvasGroup;
    private float targetLerp, targetLerpScale;
    private Vector3 startPosition;

    [SerializeField] private GameObject crystalBall;
    [SerializeField] private GameObject eventText;

    private bool initiate, startSlide, endSlide = false;    

    [SerializeField] private GameObject toDesactive;

    [Header("Light")]
    private float exposureStart = 1.681691f;
    [SerializeField] private float exposureEnd = 1.25f;

    [SerializeField] private Light ambianceLight;
    private Color startColor;
    [SerializeField] private Color endColor;

    private void OnEnable()
    {
        initiate = true;
    }


    private void Start()
    {
        startColor = ambianceLight.color;
        canvasGroup = GetComponent<CanvasGroup>();
        startPosition = eventText.transform.position;
    }

    private void Update()
    {
        if (initiate) 
            StartUI();

        if (startSlide)
            Slide();

        if (endSlide)
            EndUI();
    }

    public void StartUI()
    {
        if (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime;
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp (exposureStart, exposureStart - exposureEnd, canvasGroup.alpha));
            ambianceLight.color = Color.Lerp(startColor, endColor, canvasGroup.alpha);
        }
        else
        {
            initiate = false;
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
        if (eventText.transform.localScale.x > 0.01)
        {
            targetLerp += Time.deltaTime * 0.1f;
            targetLerpScale += Time.deltaTime;
        }
        else
        {
            canvasGroup.alpha = 0f;
            targetLerp = 0f;
            targetLerpScale = 0f;

            startSlide = false;
        }

        var minPos = eventText.transform.position;
        var maxPos = crystalBall.transform.position;

        var targetPosition = Vector3.Lerp(minPos, maxPos, targetLerp);
        eventText.transform.position = targetPosition;

        var targetScale = Mathf.Lerp(1, 0, targetLerpScale);
        eventText.transform.localScale = new Vector3 (targetScale, targetScale, 0);
    }

    public void EndEvent()
    {
        endSlide = true;

        if (startSlide)
        {
            startSlide = false;
            canvasGroup.alpha = 0f;
            targetLerp = 0f;
            targetLerpScale = 0f;
        }
    }

    private void EndUI()
    {
        
        if (targetLerp < 1) 
        {
            targetLerp += Time.deltaTime;
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(exposureStart - exposureEnd, exposureStart, targetLerp));
            ambianceLight.color = Color.Lerp(endColor, startColor, targetLerp);
        }
        else
        {
            endSlide = false;
            targetLerp = 0;
            eventText.transform.localScale = new Vector3(1, 1, 1);
            eventText.transform.position = startPosition;
            toDesactive.SetActive(false);
        }
    }
}
