using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelperScripts.EventSystem;
using TMPro;

public class EventManager : MonoBehaviour
{
    // Singleton
    private static EventManager instance;
    public static EventManager Instance { get => instance; }

    [Header("Event")]
    [SerializeField] private EventScriptable onEventCancel;
    [SerializeField] private EventScriptable onEventEnd;
    [SerializeField] private EventScriptable onPrevivewDeactivated;
    [SerializeField] private EventScriptable onPiecePlaced;
    [SerializeField] private ListOfGameplayEvent eventListRandom;
    private GameplayEvent currentEventSO;
    public bool IsEventActive { get => isEventActive;}

    private bool isEventActive;

    [Header("Scene references")]
    [SerializeField] Image eventImage;
    [SerializeField] TextMeshProUGUI textCoolDown;


    //[Header("CoolDown")]
    private int currentCoolDown;
    private bool mustUseEvent;

    //[Header("Datas from Grid3DManager")]
    public PieceSO CurrentPieceSO { get => currentPieceSO; }
    private PieceSO currentPieceSO;
    private PieceSO nextPieceSO;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    private void Start()
    {
        onEventEnd.AddListener(SwitchEvent);
        onPiecePlaced.AddListener(UpdateCoolDown);
        GetRandomEvent();
    }


    private void GetRandomEvent()
    {
        currentEventSO = eventListRandom.GetRandomEvent();
        eventImage.sprite = currentEventSO.eventSprite;
        currentCoolDown = currentEventSO.cooldown;

        if (currentCoolDown == 0) ForceEvent();
        else if (currentCoolDown > 0) currentCoolDown++;
        UpdateCoolDownVisual();
        
    }

    public void EventButton()
    {
        if (!isEventActive)
        {
            ActivateEvent();
        }
        else
        {
            if (mustUseEvent) return;
            onEventCancel.Call();
            CancelEvent();
        }
    }

    public void CallOnPreviewDeactivated()
    {
        onPrevivewDeactivated.Call();
    }

    public void ActivateEvent()
    {
        isEventActive = true;

        currentEventSO.Activate();
    }

    public void CancelEvent()
    {
        isEventActive = false;
        mustUseEvent = false;
        currentEventSO.Deactivate();
    }

    public void DeactivateEvent()
    {
        currentEventSO.EndEvent();
        CancelEvent();
    }

    private void SwitchEvent()
    {
        if (!IsEventActive) return;
        DeactivateEvent();
        GetRandomEvent();
    }


    public void GetPieceToSave()
    {
        currentPieceSO = Grid3DManager.Instance.CurrentPiece;
        nextPieceSO = Grid3DManager.Instance.NextPiece;
    }
    public void SetSavedPiece(PieceSO _current, PieceSO _next)
    {
        Grid3DManager.Instance.ChangePieceSO(_current, _next);
    }

    public void SetSavedPiece()
    {
        Grid3DManager.Instance.ChangePieceSO(currentPieceSO, nextPieceSO);
    }
    public void UpdateCoolDown()
    {
        if (currentCoolDown > 0)
        {
            currentCoolDown--;
            if (currentCoolDown == 0) ForceEvent();
        }

        UpdateCoolDownVisual();
    }
    public void UpdateCoolDownVisual()
    {
        if (currentCoolDown < 0) textCoolDown.text = "-";
        else textCoolDown.text = currentCoolDown.ToString();
    }


    private void ForceEvent()
    {
        currentCoolDown = 0;
        mustUseEvent = true;
        ActivateEvent();
    }

}
