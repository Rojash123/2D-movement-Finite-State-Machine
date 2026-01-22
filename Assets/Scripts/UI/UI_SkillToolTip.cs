using System.Collections;
using System.Drawing;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;
    private string lockedSkillText = "You've taken a different path for this skill & This skill is locked now";

    [SerializeField] private string metCondition;
    [SerializeField] private string notMetCondition;
    [SerializeField] private string importantAnnouncement;

    Coroutine texteffectCo;

    protected override void Awake()
    {
        base.Awake();
        ui=GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);

    }

    public override void ShowToolTip(bool show, RectTransform targetRectTransform)
    {
        base.ShowToolTip(show, targetRectTransform);

    }
    public void ShowToolTip(bool show, RectTransform targetRectTransform, UI_Treenode treeNode)
    {
        base.ShowToolTip(show, targetRectTransform);
        skillName.text = treeNode.SkillData.skillName;
        skillDescription.text = treeNode.SkillData.Description;

        string skillLockedText = GetColouredText(importantAnnouncement, lockedSkillText);
        string textToSend=treeNode.isLocked?skillLockedText: GetRequirements(treeNode.SkillData.cost, treeNode.neededNode, treeNode.conflictingNode);

        skillRequirements.text = textToSend;
    }

    private string GetRequirements(int skillCost, UI_Treenode[] neededNodes, UI_Treenode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Requirements:");

        string color = skillTree.HasEnoughSkillPoints(skillCost) ? metCondition : notMetCondition;
        sb.AppendLine($"<color={color}> {skillCost} Skill Point(s)</color>");

        foreach (var item in neededNodes)
        {
            string nodeColor = item.isUnlocked ? metCondition : notMetCondition;
            sb.AppendLine($"<color={nodeColor}> {item.SkillData.skillName}</color>");
        }

        if (conflictNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine();
        sb.AppendLine("Locks Out");
        foreach (var item in conflictNodes)
        {
            sb.AppendLine($"<color={importantAnnouncement}> {item.SkillData.skillName}</color>");
        }

        return sb.ToString();
    }


    public void LockedSKillEffect()
    {
        if(texteffectCo!=null)
            StopCoroutine(texteffectCo);

        texteffectCo = StartCoroutine(TextBlinkEffectCO(skillRequirements, 0.15f,3));
    }

    private IEnumerator TextBlinkEffectCO(TextMeshProUGUI text,float blinkInterVal,float blinkCount)
    {
        for (int i = 0; i < blinkCount; i++) 
        {
            text.text = GetColouredText(notMetCondition, lockedSkillText);
            yield return new WaitForSeconds(blinkInterVal);

            text.text = GetColouredText(importantAnnouncement, lockedSkillText);
            yield return new WaitForSeconds(blinkInterVal);
        }
    }
}
