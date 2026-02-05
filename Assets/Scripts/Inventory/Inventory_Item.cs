using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    private string itemId;

    public ItemDataSO itemData;
    public int stackSize = 1;
    public ItemModifier[] modifiers { get; set; }

    public ItemEffectDataSO itemEffect;

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        modifiers=EquipmentData()?.modifiers;
        itemEffect=itemData.itemEffect;
        itemId=itemData.itemName+Guid.NewGuid();
    }
    public EquitmentDataSO EquipmentData()
    {
        if (itemData is EquitmentDataSO equipment)
            return equipment;

        return null;
    }

    public void AdditemEffect(Player player) => itemEffect?.Subscribe(player);
    public void RemoveitemEffect() => itemEffect?.UnSubscribe();

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mods in modifiers)
        {
            Stat statToModify = playerStats.GetStat(mods.statType);
            statToModify.AddModifier(mods.value,itemId);
        }
    }
    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var mods in modifiers)
        {
            Stat statToModify = playerStats.GetStat(mods.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    public bool CanStack() => stackSize < itemData.maxSize;
    public void AddStack() => stackSize++;
    public void Removetack() => stackSize--;

}
