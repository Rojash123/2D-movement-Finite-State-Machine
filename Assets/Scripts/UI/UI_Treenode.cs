using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Treenode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectionHandler treeConnectionHandler;

    [Header("Unlock Details")]
    public UI_Treenode[] neededNode;
    public UI_Treenode[] conflictingNode;
    public bool isUnlocked;
    public bool isLocked;


    [Header("Skill Details")]
    [SerializeField] private SkillData_SO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    private string lockedColorCode = "#A8A8A8";
    public SkillData_SO SkillData => skillData;


    private Color lastColor;

    private void Awake()
    {
        UpdateColor(GetColorByHex(lockedColorCode));
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        treeConnectionHandler = GetComponent<UI_TreeConnectionHandler>();
    }

    private void Start()
    {
        if (skillData.unlockByDefault)
            Unlock();
    }
    private void OnValidate()
    {
        if (skillData == null) return;

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.skillIcon;
        skillCost = skillData.cost;
        gameObject.name = $"UI_TreeNode-{skillName}";
    }
    private void OnDisable()
    {
        if (isLocked)
            UpdateColor(GetColorByHex(lockedColorCode));

        if (isUnlocked)
            UpdateColor(Color.white);
    }

    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        skillTree.AddSkillPoints(skillData.cost);
        treeConnectionHandler.UnlockConnectionImage(false);
    }

    private void Unlock()
    {
        isUnlocked = true;

        UpdateColor(Color.white);
        LockedConflictNode();

        skillTree.RemoveSkillPoint(skillData.cost);
        treeConnectionHandler.UnlockConnectionImage(true);

        skillTree.skillManager.GetSkillByType(SkillData.skillType).SetSkillUpgrade(SkillData.upgradeData);
    }

    private bool CanbeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        foreach (var node in neededNode)
            if (!node.isUnlocked) return false;

        foreach (var node in conflictingNode)
            if (node.isUnlocked) return false;

        if (!skillTree.HasEnoughSkillPoints(skillData.cost))
            return false;

        return true;
    }
    private void LockedConflictNode()
    {
        foreach (var node in conflictingNode)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    private void UpdateColor(Color color)
    {
        if (!skillIcon) return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    private void ToggleNodeHighLights(bool highlight)
    {
        Color highlightColor = Color.white * 0.9f; highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastColor;
        UpdateColor(colorToApply);
    }

    public void LockChildNodes()
    {
        isLocked = true;
        foreach (var node in treeConnectionHandler.GetChildNodes())
        {
            node.LockChildNodes();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.toolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked) return;

        ToggleNodeHighLights(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.toolTip.ShowToolTip(false, rect);

        if (isUnlocked || isLocked) return;

        ToggleNodeHighLights(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanbeUnlocked())
            Unlock();
        else if (isLocked)
            ui.toolTip.LockedSKillEffect();
    }

    private Color GetColorByHex(string hexCode)
    {
        ColorUtility.TryParseHtmlString(hexCode, out Color color);
        return color;
    }
}
