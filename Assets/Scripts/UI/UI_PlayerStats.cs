using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] UI_StatSlots;
    private Inventory_Player inventory;

    private void Awake()
    {
        UI_StatSlots=GetComponentsInChildren<UI_StatSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChanged += UpdateStatusUI;
    }

    private void Start()
    {
        UpdateStatusUI();
    }
    private void OnDestroy()
    {
        inventory.OnInventoryChanged -= UpdateStatusUI;
    }

    private void UpdateStatusUI()
    {
        foreach (var slot in UI_StatSlots)
            slot.UpdateStatValue();
    }
}
