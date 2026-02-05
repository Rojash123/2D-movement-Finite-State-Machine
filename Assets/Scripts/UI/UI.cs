using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip toolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_Stattooltip statToolTip { get; private set; }

    public UI_SkillTree skillTree { get; private set; }
    public UI_Inventory inventory { get; private set; }

    private bool skillTreeEnabled;
    private bool inventoryEnabled;

    private void Awake()
    {
        toolTip = GetComponentInChildren<UI_SkillToolTip>();
        itemToolTip= GetComponentInChildren<UI_ItemToolTip>();
        statToolTip= GetComponentInChildren<UI_Stattooltip>();
        skillTree= GetComponentInChildren<UI_SkillTree>(true);
        inventory= GetComponentInChildren<UI_Inventory>(true);

        skillTreeEnabled = skillTree.gameObject.activeSelf;
        inventoryEnabled = inventory.gameObject.activeSelf;
    }
    public void ToggleSkillTree()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        toolTip.ShowToolTip(false,null);
    }
    public void ToggleInventory()
    {
        inventoryEnabled = !inventoryEnabled;
        inventory.gameObject.SetActive(inventoryEnabled);
        itemToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }
}
