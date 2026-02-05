using TMPro;
using UnityEngine;

public class UI_Stattooltip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindAnyObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public override void ShowToolTip(bool show, RectTransform targetRectTransform)
    {
        base.ShowToolTip(show, targetRectTransform);
    }
    public void ShowToolTip(bool show, RectTransform targetRectTransform,StatType statType)
    {
        base.ShowToolTip(show, targetRectTransform);
        statToolTipText.text = GetstatTextByType(statType);
    }
    public string GetstatTextByType(StatType statType)
    {
        return statType.ToString();
    }
}
