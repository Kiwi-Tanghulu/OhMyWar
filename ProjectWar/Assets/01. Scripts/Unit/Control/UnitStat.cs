using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UnitStat : UnitComponent
{
    private Dictionary<UnitStatType, Stat> stats;

    public override void InitCompo(UnitController _controller)
    {
        base.InitCompo(_controller);

        stats = new();

        Type soType = controller.Info.GetType();
        foreach(UnitStatType type in Enum.GetValues(typeof(UnitStatType)))
        {
            int value = (int)soType.GetField(type.ToString()).GetValue(controller.Info);
            stats.Add(type, new Stat(value));
        }
    }

    public int GetStat(UnitStatType type)
    {
        return stats[type].GetValue();
    }

    public void AddModifier(UnitStatType type, int value)
    {
        stats[type].AddModifier(value);
    }

    public void RemoveModifier(UnitStatType type, int value)
    {
        stats[type].RemoveModifier(value);
    }
}
