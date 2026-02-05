using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public override void ShowToolTip(bool show, RectTransform targetRectTransform)
    {
        base.ShowToolTip(show, targetRectTransform);
    }
    public void ShowToolTip(bool show, RectTransform targetRectTransform, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetRectTransform);
        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
        {
            return "used for crafting";
        }
        if (item.itemData.itemType == ItemType.Consumable) 
        {
            return item.itemData.itemEffect.effectDescription;
        }

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("");

        foreach (var mod in item.modifiers)
        {
            string percentage = IsPercentage(mod.statType) ? "%" : "";
            stringBuilder.AppendLine($"+{mod.statType} {mod.value}{percentage}");
        }
        return stringBuilder.ToString();
    }

    private bool IsPercentage(StatType type)
    {
        switch (type)
        {
            case StatType.CriticalChance:
            case StatType.CriticalPower:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.lightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;

            default: return false;
        }
    }
}
