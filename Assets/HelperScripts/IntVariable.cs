﻿using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntVariable", menuName = "UnityHelperScripts/Variables/IntVariable", order = 0)]
public class IntVariable : ScriptableObject 
{
    public int value;
    public int defaultValue;
    public UnityEvent OnValueChanged;
    public static implicit operator int(IntVariable reference)
    {
            return reference.value;
    }

    public void SetValue(int v)
    {
        this.value = v;
        OnValueChanged?.Invoke();
    }

    public void Add(int v)
    {
        this.value += v;
        OnValueChanged?.Invoke();
    }

    public override string ToString(){
        return value.ToString();
    }

    public void Reset()
    {
        value = defaultValue;
    }
}