using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> mods = new List<StatModifier>();
    
    private float finalValue;
    private bool needsRecalculate=true;
    public float GetValue =>needsRecalculate?GetFinalValue():finalValue;

    public void AddModifier(float value, string source)
    {
        StatModifier modifier=new(value,source);
        mods.Add(modifier);
        needsRecalculate=true;
    }
    public void RemoveModifier(string source)
    {
        mods.RemoveAll(x=>x.source==source);
        needsRecalculate = true;
    }
    public float GetFinalValue()
    {
        needsRecalculate = false;
        finalValue = baseValue;
        foreach (StatModifier modifier in mods)
        {
            finalValue += modifier.value;
        }
        return finalValue;
    }

    public void SetBaseValue(float value)=>baseValue = value;
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
