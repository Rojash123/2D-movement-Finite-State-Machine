using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip toolTip;
    public UI_SkillTree skillTree;
    private bool skillTreeEnabled;

    private void Awake()
    {
        toolTip = GetComponentInChildren<UI_SkillToolTip>();
        skillTree= GetComponentInChildren<UI_SkillTree>(true);
    }
    public void ToggleSkillTree()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        toolTip.ShowToolTip(false,null);
    }
}
