using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public enum TypeEvent
    {
        Lightning,
        Orc,
    }


    // Singleton
    private static EventManager instance;
    public static EventManager Instance { get => instance; }

    [Header("Event")]
    [SerializeField] private EventScriptable onEventCancel;
    [SerializeField] private EventScriptable onEventEnd;
    [SerializeField] private EventScriptable onPrevivewDeactivated;
    [SerializeField] private ListOfGameplayEvent eventListRandom;
    private GameplayEvent currentEventSO;
    public bool IsEventActive { get => isEventActive;}

    private bool isEventActive;

    [Header("Scene references")]
    [SerializeField] Image eventImage;

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

        GetRandomEvent();
    }

    private void Start()
    {
        onEventEnd.AddListener(SwitchEvent);
    }


    private void GetRandomEvent()
    {
        currentEventSO = eventListRandom.GetRandomEvent();
        eventImage.sprite = currentEventSO.eventSprite;
    }

    public void EventButton()
    {
        if (!isEventActive)
        {
            ActivateEvent();
        }
        else
        {
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
}
