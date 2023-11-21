using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Singleton
    private static EventManager instance;
    public static EventManager Instance { get => instance; }

    public enum TypeEvent
    {
        Lightning,
    }

    [SerializeField] private ListOfEventSO eventListRandom;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }
}
