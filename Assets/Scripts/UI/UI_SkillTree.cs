using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectionHandler[] connectionHandlers;

    public void RemoveSkillPoint(int cost) => skillPoints -= cost;
    public bool HasEnoughSkillPoints(int cost) => skillPoints >= cost;
    public void AddSkillPoints(int cost) => skillPoints += cost;

    public void RefundAllSkills()
    {
        UI_Treenode[] skillNodes =GetComponentsInChildren<UI_Treenode>();

        foreach (UI_Treenode skillNode in skillNodes)
            skillNode.Refund();
    }


    [ContextMenu("Handle Connections")]
    public void HandleAllConnections()
    {
        foreach (var connectionHandler in connectionHandlers)
        {
            connectionHandler.UpdateConnection();
        }
    }
}
