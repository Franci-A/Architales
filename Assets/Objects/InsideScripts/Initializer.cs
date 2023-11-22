using System;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

public class Initializer : MonoBehaviour
{
    #region Singleton
    private static Initializer instance;
    #endregion

    [SerializeField] private List<Object> toInitialize = new List<Object>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeObjects();
    }

    private void InitializeObjects()
    {
        for (int i = 0; i < toInitialize.Count; ++i)
        {
            if (toInitialize[i] is InitializeOnAwake component)
            {
                component.Initialize();
                continue;
            }

            throw new Exception($"{toInitialize[i].name} does not Implement the interface {nameof(InitializeOnAwake)}");
        }
    }
}
