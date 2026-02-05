using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item itemInSlot { get; private set; }

    protected Inventory_Player inventory;
    protected RectTransform rectTransform;
    protected UI ui;

    [Header("Item Slot Details")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    protected void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();
        ui = GetComponentInParent<UI>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white; color.a = 0.9f;
        itemIcon.color = color;
        itemIcon.sprite = item.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null || itemInSlot.itemData.itemType == ItemType.Material)
            return;

        if (itemInSlot.itemData.itemType == ItemType.Consumable)
        {
            if (!itemInSlot.itemEffect.CanbeUsed()) return;

            inventory.TryUseItem(itemInSlot);
        }
        else
            inventory.TryEquipItem(itemInSlot);

        if (itemInSlot == null)
            ui.itemToolTip.ShowToolTip(false, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowToolTip(true, rectTransform, itemInSlot);
    }
}
