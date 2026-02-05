using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private UI_ItemSlot[] itemSlots;
    [SerializeField] private UI_EquipSlot[] equipSlots;

    [SerializeField] private Transform itemSlotsParents;
    [SerializeField] private Transform equipSlotsParents;

    private Inventory_Player inventory;

    private void Awake()
    {
        itemSlots = itemSlotsParents.GetComponentsInChildren<UI_ItemSlot>();
        equipSlots = equipSlotsParents.GetComponentsInChildren<UI_EquipSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChanged += UpdateUI;

        UpdateUI();
    }
    private void OnDestroy()
    {
        inventory.OnInventoryChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        UpdateEquipmentSlots();
        UpdateInventorySlots();
    }
    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> itemList = inventory.equipList;
        for (int i = 0; i < equipSlots.Length; i++)
        { 
            var equipSlot = itemList[i];
            if (!equipSlot.HasItem())
            {
                equipSlots[i].UpdateSlot(null);
            }
            else
            {
                equipSlots[i].UpdateSlot(equipSlot.equippedItem);
            }
        }
    }

    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < itemList.Count)
            {
                itemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                itemSlots[i].UpdateSlot(null);
            }
        }
    }
}
