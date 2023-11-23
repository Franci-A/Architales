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
    [SerializeField] private EventScriptable onPiecePlaced;
    [SerializeField] private ListOfEventSO eventListRandom;
    private EventSO currentEventSO;
    public bool IsEventActive { get => isEventActive;}
    private bool isEventActive;

    [Header("Scene references")]
    [SerializeField] Image eventImage;

    //[Header("Datas from Grid3DManager")]
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
        onPiecePlaced.AddListener(SwitchEvent);
    }


    private void GetRandomEvent()
    {
        currentEventSO = eventListRandom.GetRandomEvent();
        eventImage.sprite = currentEventSO.eventSprite;
    }

    public void EventButton()
    {
        if (!isEventActive) ActivateEvent();
        else DeactivateEvent();
    }

    public void ActivateEvent()
    {
        isEventActive = true;
        if(currentEventSO.eventType == TypeEvent.Lightning || currentEventSO.eventType == TypeEvent.Orc)
        {
            GetPieceToSave();
            SetSavedPiece(currentEventSO.piece, currentPieceSO);
        }
    }

    public void DeactivateEvent()
    {
        isEventActive = false;
        SetSavedPiece(currentPieceSO, nextPieceSO);
    }

    private void SwitchEvent()
    {
        if (!IsEventActive) return;
        DeactivateEvent();
        GetRandomEvent();
    }


    private void GetPieceToSave()
    {
        currentPieceSO = Grid3DManager.Instance.CurrentPiece;
        nextPieceSO = Grid3DManager.Instance.NextPiece;
    }
    private void SetSavedPiece(PieceSO _current, PieceSO _next)
    {
        Grid3DManager.Instance.ChangePieceSO(_current, _next);
    }
}
