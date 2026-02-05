using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EquitmentDataSO", menuName = "Scriptable Objects/EquitmentDataSO")]
public class EquitmentDataSO : ItemDataSO
{
    [Header("Item Modifiers")]
    public ItemModifier[] modifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}
