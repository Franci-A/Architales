using System;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

public class Initializer : MonoBehaviour
{
    #region Singleton
    private static Initializer instance;
    #endregion

    [Serializable]
    private struct ObjectToInitialize
    {
        public Object obj;
        public bool InitializeOnAwake;
        public bool ResetOnDisable;
    }

    [SerializeField] private List<ObjectToInitialize> objects = new List<ObjectToInitialize>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        InitializeObjects();
    }

    private void OnDisable()
    {
        ResetObjects();
    }

    private void InitializeObjects()
    {
        foreach (ObjectToInitialize initObject in objects)
        {
            if (!initObject.InitializeOnAwake)
                continue;

            if (initObject.obj is InitializeOnAwake component)
            {
                component.Initialize();
                continue;
            }

            throw new MissingIntefaceException<InitializeOnAwake>(initObject.obj);
        }
    }

    private void ResetObjects()
    {
        foreach (ObjectToInitialize initObject in objects)
        {
            if (!initObject.ResetOnDisable)
                continue;

            if (initObject.obj is UninitializeOnDisable component)
            {
                component.Uninitialize();
                continue;
            }

            throw new MissingIntefaceException<UninitializeOnDisable>(initObject.obj);
        }
    }
}
