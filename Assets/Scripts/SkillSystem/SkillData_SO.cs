using UnityEngine;

[CreateAssetMenu(fileName = "SkillData_SO", menuName = "Scriptable Objects/SkillData_SO")]
public class SkillData_SO : ScriptableObject
{
    public int cost;

    [Header("Skill Description")]
    public string skillName;
    [TextArea]
    public string Description;
    public Sprite skillIcon;
}
