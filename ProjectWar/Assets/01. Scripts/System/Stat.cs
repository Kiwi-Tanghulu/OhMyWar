using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int value;
    [SerializeField] private List<int> modifiers;
    public event Action<int> OnValueChangeEvent;

    public Stat(int defaultValue)
    {
        value = defaultValue;
        modifiers = new List<int>();
    }

    public int GetValue()
    {
        int v = value;

        for(int i = 0; i < modifiers.Count; ++i)
        {
            v += modifiers[i];
        }

        return v;
    }

    public void AddModifier(int value)
    {
        if (value != 0)
        {
            modifiers.Add(value);
            OnValueChangeEvent?.Invoke(GetValue());
        }
    }

    public void RemoveModifier(int value)
    {
        if(value != 0)
        {
            modifiers.Remove(value);
            OnValueChangeEvent?.Invoke(GetValue());
        }
    }
}
