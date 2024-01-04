using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float value;
    [SerializeField] private List<float> modifiers;
    public event Action<float> OnValueChangeEvent;

    public Stat(float defaultValue)
    {
        value = defaultValue;
        modifiers = new List<float>();
    }

    public float GetValue()
    {
        float v = value;

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
