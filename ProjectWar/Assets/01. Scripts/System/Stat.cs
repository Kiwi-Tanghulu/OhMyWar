using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    private int value;
    private List<int> modifiers;

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
            modifiers.Add(value);
    }

    public void RemoveModifier(int value)
    {
        if(value != 0)
            modifiers.Remove(value);
    }
}
