using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryChanged;

    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {

    }
    public void EquipItem(Inventory_Item item)
    {
    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item=>item==itemToUse);

        if (consumable == null) return;

        consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
        {
            consumable.Removetack();
        }
        else
        {
            RemoveItem(consumable);
        }
        OnInventoryChanged?.Invoke();
    }
    public bool CanAdd() => itemList.Count < maxInventorySize;
    public Inventory_Item FindStackable(Inventory_Item item)
    {
        List<Inventory_Item> stackableItem = itemList.FindAll(x => x.itemData == item.itemData);
        foreach (var stack in stackableItem)
        {
            if (stack.CanStack())
                return stack;
        }
        return null;
    }
    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item item = FindStackable(itemToAdd);
        if (item != null)
            item.AddStack();
        else
            itemList.Add(itemToAdd);


        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(Inventory_Item itemToRemove)
    {
        itemList.Remove(itemToRemove);
        OnInventoryChanged?.Invoke();
    }

    public Inventory_Item FindItem(ItemDataSO itemData)
    {
        return itemList.Find(x => x.itemData == itemData);
    }

    public void TriggerUIUpdate()=>OnInventoryChanged?.Invoke();
}
