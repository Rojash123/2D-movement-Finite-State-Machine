using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public Player player;
    public List<Inventory_EquipmentSlot> equipList;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }
    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        var matchingSlots = equipList.FindAll(x => x.slotType == item.itemData.itemType);

        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(item, slot);
                return;
            }
        }

        var slotToReplace = matchingSlots[0];
        var unEquipItem = slotToReplace.equippedItem;

        UnequipItem(unEquipItem, slotToReplace!=null);
        EquipItem(item, slotToReplace);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHealthPercentage = player.playerHealth.GetHealthPercentage();

        slot.equippedItem = itemToEquip;
        slot.equippedItem.AddModifiers(player.entityStats);
        slot.equippedItem.AdditemEffect(player);

        player.playerHealth.SetHealthPercentage(savedHealthPercentage);

        RemoveItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnEquip,bool replaceItem=false)
    {
        if (CanAdd() == false && replaceItem==false)
        {
            return;
        }
        float savedHealthPercentage = player.playerHealth.GetHealthPercentage();


        var slotToUnEquip = equipList.Find(x => x.equippedItem == itemToUnEquip);
        if (slotToUnEquip != null)
            slotToUnEquip.equippedItem = null;
       
        itemToUnEquip.RemoveModifiers(player.entityStats);
        itemToUnEquip.RemoveitemEffect();
        player.playerHealth.SetHealthPercentage(savedHealthPercentage);
        AddItem(itemToUnEquip);
    }
}
